using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Pool;

public class BuildingSystem : MonoBehaviour
{
    public int width;
    public int height;
    public int tileSize;
    public bool combatDisabled;

    public Camera mainCamera;
    public Camera buildCamera;
    public GameObject mainUI;
    public Canvas BldOverlay;
    public GameObject Maincanvasgroup;
    public GameObject City;
    public GameObject m_blankTile;
    public GameObject m_islandTile;
    public GameObject m_defaultSelectionMarker;

    private GameObject m_selectionMarker;
    public Building[] m_buildings;
    private EBuildings m_selectedBuilding = EBuildings.NULL;

    public BuildGrid m_buildGrid = null;
    public CitySave_Loading CitySaver = new CitySave_Loading();
    private CameraController m_cameraController = null;

    private bool m_buildModeEnabled = false;

    private Quaternion m_buildingRotation = new Quaternion(0f, 0f, 0f, 0f);
    private float m_valToRotate = 0f;

    private Cities m_city;

    //Resource UI
    public Text m_foodText;
    public Text m_peopleText;
    public Text m_materialsText;

    [System.Serializable]
    public enum EBuildings
    {
        //buildings that don't affect combat        
        BUILDING_HUT,
        BUILDING_FARM,
        BUILDING_WORKSHOP,
        //combat buildings
        BUILDING_SAIL,
        BUILDING_DIVINGSTATION,
        BUILDING_REFINERY,
        //combat weapons
        BUILDING_GUARDTOWER,
        BUILDING_TREBUCHET,
        //etc.
        
        STATIONARY_ISLAND,

        DELETE,
        NULL
    }
    [System.Serializable]
    public struct Building
    {
        public GameObject model;
        public GameObject hoverModel;
        public int materialCost;
        public string description;
    }



    // Initialise.
    void Start()
    {
        m_cameraController = GetComponent<CameraController>();
        BldOverlay.enabled = false;
        m_buildGrid = new BuildGrid(width, height, tileSize, m_blankTile, City.transform);

        BuildButton.setBuildingSystem(this);


        m_selectionMarker = LeanPool.Spawn(m_defaultSelectionMarker);
        m_selectionMarker.SetActive(false);

        m_city = City.GetComponent<Cities>();
        if (StaticVals.FirstTime)
        {

            loadCity(true);         
            StaticVals.FirstTime = false;
        }
        else
        {
            loadCity();
        }

        /*
        //Debug - Spawn a few buildings when the scene is initialised
        Vector2Int gridRef = new Vector2Int(10, 10);
        Vector3 gridPos = m_buildGrid.getWorldPos(gridRef);
        placeBuilding(EBuildings.BUILDING_DIVINGSTATION, gridRef, gridPos);
        gridRef = new Vector2Int(10, 11);
        gridPos = m_buildGrid.getWorldPos(gridRef);
        placeBuilding(EBuildings.BUILDING_FARM, gridRef, gridPos);
        gridRef = new Vector2Int(14, 11);
        gridPos = m_buildGrid.getWorldPos(gridRef);
        placeBuilding(EBuildings.BUILDING_GUARDTOWER, gridRef, gridPos);
        gridRef = new Vector2Int(14, 14);
        gridPos = m_buildGrid.getWorldPos(gridRef);
        placeBuilding(EBuildings.BUILDING_SAIL, gridRef, gridPos);
        */
        saveCity();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && combatDisabled)
        {
            ToggleBuildMode();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            BuildRotate();
        }

        if (m_buildModeEnabled)
        {
            MouseRaycast();
        }
    }

    // Track mouse position on screen and highlight tile hovered over.
    private void MouseRaycast()
    {
        Ray ray = buildCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject currentHit = hit.collider.transform.gameObject;
            if (currentHit.tag == "BuildTile")
            {
                Debug.DrawLine(ray.origin, hit.point);
                Vector2Int gridRef = m_buildGrid.TranslateWorldToGridPos(hit.point);
                Vector3 gridPos = m_buildGrid.getWorldPos(gridRef);
                               
                m_selectionMarker.SetActive(true);
                m_selectionMarker.transform.position = gridPos;
                m_selectionMarker.transform.rotation = m_buildingRotation;

                if (Input.GetMouseButtonDown(0) && m_selectedBuilding != EBuildings.NULL)
                {
                    placeBuilding(m_selectedBuilding, gridRef, gridPos);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    m_selectedBuilding = EBuildings.NULL;
                    LeanPool.Despawn(m_selectionMarker);
                    m_selectionMarker = LeanPool.Spawn(m_defaultSelectionMarker);
                }
            }
            else
            {
                m_selectionMarker.SetActive(false);
            }
        }
        else
        {
            m_selectionMarker.SetActive(false);
        }
    }
    
    public void saveCity()
    {
        m_buildGrid.saveCity(CitySaver);
    }

    public void loadCity(bool first = false)
    {
        EBuildings[,] loadedCity;
        if (first)
        {
            loadedCity = CitySaver.Load("/playerCity", m_buildGrid.width, true);
        }
        else
        {
            loadedCity = CitySaver.Load("/playerCity", m_buildGrid.width);
        }

        for (int x = 0; x < m_buildGrid.width; x++)
        {
            for (int y = 0; y < m_buildGrid.height; y++)
            {
                if(loadedCity[x, y] != EBuildings.NULL)
                {
                    Vector2Int pos = new Vector2Int(x, y);

                    Vector3 gridPos = m_buildGrid.getWorldPos(pos);
                    placeBuilding(loadedCity[x, y], pos, gridPos, false);
                }

            }
        }
    }

    public void placeBuilding(EBuildings building, Vector2Int gridRef, Vector3 gridPos, bool usingCost = true)
    {
        int buildingCost;
        if (building != EBuildings.DELETE && building != EBuildings.STATIONARY_ISLAND)
        {
            buildingCost = m_buildings[(int)building].materialCost;
        }
        else buildingCost = -5;

        EBuildings gridBuilding = m_buildGrid.getTileBuilding(gridRef);
        if (building != EBuildings.DELETE && gridBuilding == EBuildings.NULL) 
        {

            
            if (buildingCost <= GameManager.obj().m_resources.GetResourceValue("materials")) 
            {
                LeanPool.Despawn(m_buildGrid.getTileObj(gridRef));

                GameObject obj = LeanPool.Spawn(m_buildings[(int)building].model, gridPos, m_buildingRotation, City.transform);
                obj.tag = "BuildTile";
                m_buildGrid.setTileBuilding(gridRef, building, obj);
                m_selectionMarker.transform.rotation = m_buildingRotation;

                AdjacencyChecks(gridRef, true);
                if (usingCost)
                {
                    GameManager.obj().m_resources.SetResourceValue("materials", -buildingCost);
                }
            }
        }
        else if (m_selectedBuilding == EBuildings.DELETE) {
            LeanPool.Despawn(m_buildGrid.getTileObj(gridRef));
            GameObject obj = LeanPool.Spawn(m_blankTile, gridPos, m_buildingRotation, City.transform);
            m_buildGrid.setTileBuilding(gridRef, EBuildings.NULL, obj);
            AdjacencyChecks(gridRef, false);
            if (usingCost)
            {
                GameManager.obj().m_resources.SetResourceValue("materials", -buildingCost);
            }
        }
    }

    // Generate a random rotation for buildings (for now).
    public void BuildRotate()
    {
        if (m_buildingRotation.y == 270f)
        {
            m_valToRotate = 0f;
        }
        else
        {
            m_valToRotate += 90f;
        }
        m_buildingRotation = Quaternion.Euler(0f, m_valToRotate, 0f);
    }

    // Checks build cost and selects building to place
    public void SelectBuildingToPlace(EBuildings buildingRef)
    {
        LeanPool.Despawn(m_selectionMarker);
        if (buildingRef >= EBuildings.DELETE)
        {
            m_selectedBuilding = buildingRef;
            m_selectionMarker = LeanPool.Spawn(m_defaultSelectionMarker);
            
        }
        else
        {
            m_selectedBuilding = buildingRef;
            Debug.Log("selected building " + buildingRef);
            
            m_selectionMarker = LeanPool.Spawn(m_buildings[(int)m_selectedBuilding].hoverModel); //add transparent model for hover
            m_selectionMarker.transform.rotation = m_buildingRotation;
           
        }  
        
    }
    public void SelectBuildingToPlace(int buildingRef)
    {
        SelectBuildingToPlace((EBuildings)buildingRef);
    }


    private void AdjacencyChecks(Vector2Int checkPos, bool visiToggle)
    {
        Vector2Int adjCheckPos = checkPos;
        EBuildings adjTileVal;

        //TODO: Make sure this check is bounds-safe
        adjCheckPos.x -= 1;
        if (adjCheckPos.x >= 0)
        {
            adjTileVal = m_buildGrid.getTileBuilding(adjCheckPos);
            toggleBlanksActive(adjCheckPos, adjTileVal, visiToggle);
        }

        adjCheckPos.x += 2;
        if (adjCheckPos.x < width)
        {
            adjTileVal = m_buildGrid.getTileBuilding(adjCheckPos);
            toggleBlanksActive(adjCheckPos, adjTileVal, visiToggle);
        }

        adjCheckPos.x = checkPos.x;
        adjCheckPos.y -= 1;
        if (adjCheckPos.y >= 0)
        {
            adjTileVal = m_buildGrid.getTileBuilding(adjCheckPos);
            toggleBlanksActive(adjCheckPos, adjTileVal, visiToggle);
        }

        adjCheckPos.y += 2;
        if (adjCheckPos.y <= height)
        {
            adjTileVal = m_buildGrid.getTileBuilding(adjCheckPos);
            toggleBlanksActive(adjCheckPos, adjTileVal, visiToggle);
        }
    }

    private void toggleBlanksActive(Vector2Int adjCheckPos, EBuildings tileVal, bool show)
    {
        if (tileVal == EBuildings.NULL)
        {
            m_buildGrid.setTileActive(adjCheckPos.x, adjCheckPos.y, m_buildModeEnabled, show);
        }        
    }

    // Switches camera, switches UI, un/pauses game
    // Updates resource counts
    private void enableBuildMode(bool enable = true)
    {
        if (m_buildModeEnabled != enable)
        {
            m_buildModeEnabled = enable;
            if (m_buildModeEnabled)
            {
                m_buildGrid.setBlankTilesVisibility(true);
                mainCamera.enabled = false;
                buildCamera.enabled = true;
                
                mainUI.SetActive(false);
                BldOverlay.enabled = true;
                BldOverlay.gameObject.SetActive(true);
                if(m_cameraController != null) m_cameraController.SetActive(true);

                //update resources
                    //get resources from movement mode
                updateResourceUI();
            }
            else
            {
                m_buildGrid.setBlankTilesVisibility(false);
                mainCamera.enabled = true;
                buildCamera.enabled = false;

                Maincanvasgroup.SetActive(true);
                mainUI.SetActive(true);
                BldOverlay.enabled = false;
                BldOverlay.gameObject.SetActive(false);
                if (m_cameraController != null) m_cameraController.SetActive(false);

                m_selectionMarker.SetActive(false);

                saveCity();               
            }
        }
    }
    public void ToggleBuildMode()
    {
        enableBuildMode(!m_buildModeEnabled);
    }

    void updateResourceUI()
    {

    }

    //private void OnMouseOver()
    //{
    //    if (trackingMouse == true)
    //    {
    //        objHighlight(true);
    //    }
    //}

    //private void OnMouseExit()
    //{
    //    objHighlight(false);
    //}

    //public void EnableHighlight(bool onOff)
    //{
    //    if (meshRenderer != null && originalMaterial != null && highlightedMaterial != null)
    //    {
    //        meshRenderer.material = onOff ? highlightedMaterial : originalMaterial;
    //    }
    //}
    public BuildGrid getBuildGrid() {
        return m_buildGrid;
    }

    public void descriptionTooltip(EBuildings buildingRef)
    {
        ToolTip.ShowTooltip_static(m_buildings[(int)buildingRef].description, m_buildings[(int)buildingRef].materialCost);
    }



}

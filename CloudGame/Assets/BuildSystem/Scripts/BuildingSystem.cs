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

    public Camera mainCamera;
    public Camera buildCamera;
    public Canvas GamOverlay;
    public Canvas BldOverlay;
    public GameObject City;
    public GameObject m_blankTile;
    public GameObject m_islandTile;
    public GameObject m_defaultSelectionMarker;
    private GameObject m_selectionMarker;
    public Building[] m_buildings;
    private EBuildings m_selectedBuilding = EBuildings.NULL;
    private Color highlightColor = new Color(1.0f, 0.7f, 0.0f);
    
    private BuildGrid m_buildGrid = null;
    private CameraController m_cameraController = null;

    private bool m_buildModeEnabled = false;

    private Quaternion m_buildingRotation = new Quaternion(0f, 0f, 0f, 0f);
    private float m_valToRotate = 0f;

    private Cities m_city;

    //Resources
    int m_resourceCountWood;
    int m_resourceCountStone;   //etc.

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
        BUILDING_ENGINE,
        //combat weapons
        BUILDING_GUARDTOWER,
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
        public int woodCost;
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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
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
    void placeBuilding(EBuildings building, Vector2Int gridRef, Vector3 gridPos) {
        EBuildings gridBuilding = m_buildGrid.getTileBuilding(gridRef);
        if (building != EBuildings.DELETE && gridBuilding == EBuildings.NULL) {
            LeanPool.Despawn(m_buildGrid.getTileObj(gridRef));

            //TODO: check + pay resource cost

            GameObject obj = LeanPool.Spawn(m_buildings[(int)building].model, gridPos, m_buildingRotation, City.transform);
            obj.tag = "BuildTile";
            m_buildGrid.setTileBuilding(gridRef, building, obj);
            m_selectionMarker.transform.rotation = m_buildingRotation;
            AdjacencyChecks(gridRef, true);
        }
        else if (m_selectedBuilding == EBuildings.DELETE) {
            LeanPool.Despawn(m_buildGrid.getTileObj(gridRef));
            GameObject obj = LeanPool.Spawn(m_blankTile, gridPos, m_buildingRotation, City.transform);
            m_buildGrid.setTileBuilding(gridRef, EBuildings.NULL, obj);
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
            if (checkBuildCost(buildingRef))
            {
                m_selectedBuilding = buildingRef;
                Debug.Log("selected building " + buildingRef);
                
                m_selectionMarker = LeanPool.Spawn(m_buildings[(int)m_selectedBuilding].hoverModel); //add transparent model for hover
                m_selectionMarker.transform.rotation = m_buildingRotation;
            }
            else m_selectedBuilding = EBuildings.NULL;
        }        
    }
    public void SelectBuildingToPlace(int buildingRef)
    {
        SelectBuildingToPlace((EBuildings)buildingRef);
    }
    bool checkBuildCost(EBuildings buildingRef)
    {
        if (buildingRef == EBuildings.NULL) return false;

        if (m_buildings[(int)buildingRef].woodCost > m_resourceCountWood) return false;
        //etc.

        return true;
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
                
                GamOverlay.enabled = false;
                BldOverlay.enabled = true;
                BldOverlay.gameObject.SetActive(true);
                if(m_cameraController != null) m_cameraController.SetActive(true);

                //Time.timeScale = 0;
                m_city.pauseMovement();

                //update resources
                    //get resources from movement mode
                updateResourceUI();
            }
            else
            {
                m_buildGrid.setBlankTilesVisibility(false);
                mainCamera.enabled = true;
                buildCamera.enabled = false;
                
                GamOverlay.enabled = true;
                BldOverlay.enabled = false;
                BldOverlay.gameObject.SetActive(false);
                if (m_cameraController != null) m_cameraController.SetActive(false);

                //Time.timeScale = 1;
                m_city.pauseMovement(false);

                m_selectionMarker.SetActive(false);

                //update resources
                    //send resources to movement mode                
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
}

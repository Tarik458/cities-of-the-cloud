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

    private Cities m_city;

    //Resources
    int m_resourceCountWood;
    int m_resourceCountStone;   //etc.

    [System.Serializable]
    public enum EBuildings
    {
        BUILDING_HUT,
        BUILDING_FARM,
        BUILDING_WORKSHOP,
        BUILDING_SAIL,
        BUILDING_GUARDTOWER,
        BUILDING_DIVINGSTATION,
        BUILDING_REFINERY,
        BUILDING_ENGINE,
        STATIONARY_ISLAND,
        //etc.

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBuildMode();
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
                EBuildings gridBuilding = m_buildGrid.getTileBuilding(gridRef);               
                m_selectionMarker.SetActive(true);
                m_selectionMarker.transform.position = gridPos;                

                if (Input.GetMouseButtonDown(0) && m_selectedBuilding != EBuildings.NULL)
                {
                    if (m_selectedBuilding != EBuildings.DELETE && gridBuilding == EBuildings.NULL)
                    {
                        LeanPool.Despawn(currentHit);
                        //check resource cost
                        GameObject obj =LeanPool.Spawn(m_buildings[(int)m_selectedBuilding].model, gridPos, m_randomRotation, City.transform);
                        obj.tag = "BuildTile";
                        m_buildGrid.setTileBuilding(gridRef, m_selectedBuilding);
                        m_randomRotation = RandBuildRotate();
                        m_selectionMarker.transform.rotation = m_randomRotation;

                        //pay resource cost
                    }
                    else if (m_selectedBuilding == EBuildings.DELETE)
                    {
                        LeanPool.Despawn(currentHit);
                        LeanPool.Spawn(m_blankTile, gridPos, m_randomRotation, City.transform);
                        m_buildGrid.setTileBuilding(gridRef, EBuildings.NULL);
                    }

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

    Quaternion m_randomRotation;
    // Generate a random rotation for buildings (for now).
    private Quaternion RandBuildRotate()
    {        
        float rotate = (float)Random.Range(0, 4) * 90f;
        return Quaternion.Euler(0f, rotate, 0f);
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
                m_randomRotation = RandBuildRotate();
                m_selectionMarker.transform.rotation = m_randomRotation;
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

    // Switches camera, switches UI, un/pauses game
    // Updates resource counts
    private void enableBuildMode(bool enable = true)
    {
        if (m_buildModeEnabled != enable)
        {
            m_buildModeEnabled = enable;
            if (m_buildModeEnabled)
            {
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public int width;
    public int height;
    public int tileSize;

    public Camera mainCamera;
    public Camera buildCamera;
    public Canvas GamOverlay;
    public Canvas BldOverlay;
    public List<GameObject> Buildings = new List<GameObject>();

    private bool trackingMouse;
    private bool removing;
    private GameObject currentHit;
    private GameObject toPlace;
    private Color highlightColor = new Color(1.0f, 0.7f, 0.0f);


    // Initialise.
    void Start()
    {
        BuildGrid BuildGrid = new BuildGrid(width, height, tileSize, Buildings);
        trackingMouse = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            CameraChange();
        }
        if (trackingMouse)
        {
            MouseRaycast(toPlace);
        }
    }

    // Track mouse position on screen and highlight tile hovered over.
    private void MouseRaycast(GameObject buildingToUse)
    {
        Ray ray = buildCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            currentHit = hit.transform.gameObject;

            if (Input.GetMouseButtonDown(0) && toPlace != null)
            {
                if (currentHit.name == "TilePlaceholder(Clone)" || removing == true)
                {
                    Instantiate(buildingToUse, currentHit.transform.position, currentHit.transform.rotation);
                    Destroy(currentHit);
                }
                toPlace = null;
            }
        }
    }

    // Allows building to place to be selected by buttons.
    public void SelectBuilding(int buttonNum)
    {
        switch (buttonNum)
        {
            case 0:
                toPlace = Buildings[0];
                removing = true;
                break;
            case 1:
                toPlace = Buildings[1];
                break;
            case 2:
                toPlace = Buildings[2];
                break;
            default:
                toPlace = null;
                break;
        }

        if(removing == true && toPlace != Buildings[0])
        {
            removing = false;
        }
    }

    // Changes camera and pauses / unpauses game.
    public void CameraChange()
    {
        switch(mainCamera.enabled)
        {
            case true:
                mainCamera.enabled = false;
                buildCamera.enabled = true;
                trackingMouse = true;
                Time.timeScale = 0;
                GamOverlay.enabled = false;
                BldOverlay.enabled = true;
                break;
            case false:
                mainCamera.enabled = true;
                buildCamera.enabled = false;
                trackingMouse = false;
                Time.timeScale = 1;
                GamOverlay.enabled = true;
                BldOverlay.enabled = false;
                break;
            default:
                mainCamera.enabled = false;
                buildCamera.enabled = true;
                trackingMouse = true;
                Time.timeScale = 0;
                GamOverlay.enabled = false;
                BldOverlay.enabled = true;
                break;
        }
    }

}

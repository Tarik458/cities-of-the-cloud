using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int width;
    public int height;
    public int tileSize;

    public Camera mainCamera;
    public Camera buildCamera; 
    public List<GameObject> Buildings = new List<GameObject>();

    private bool trackingMouse;
    private bool firstTime = true; 
    private Renderer oldRend;
    private GameObject currentHit;
    private GameObject previousHit;
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
            MouseRaycast(firstTime);
        }
    }

    // Track mouse position on screen and highlight tile hovered over.
    private void MouseRaycast(bool firstTime)
    {
        Ray ray = buildCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (firstTime)
        {
            if (Physics.Raycast(ray, out hit))
            {
                Renderer rend = hit.transform.gameObject.GetComponent<Renderer>();
                previousHit = hit.transform.gameObject;
                oldRend = previousHit.transform.gameObject.GetComponent<Renderer>();
                firstTime = false;
            }
        }

        if (Physics.Raycast(ray, out hit))
        {

            Renderer rend = hit.transform.gameObject.GetComponent<Renderer>();
            oldRend = previousHit.transform.gameObject.GetComponent<Renderer>();
            currentHit = hit.transform.gameObject;

            if (rend)
            {
                oldRend.material.color = Color.grey;
                rend.material.color = highlightColor;
            }
            previousHit = currentHit;
        }
    }

    // Changes camera and pauses / unpauses game.
    private void CameraChange()
    {
        switch(mainCamera.enabled)
        {
            case true:
                mainCamera.enabled = false;
                buildCamera.enabled = true;
                trackingMouse = true;
                Time.timeScale = 0;
                break;
            case false:
                mainCamera.enabled = true;
                buildCamera.enabled = false;
                trackingMouse = false;
                Time.timeScale = 1;
                break;
            default:
                mainCamera.enabled = false;
                buildCamera.enabled = true;
                trackingMouse = true;
                Time.timeScale = 0;
                break;
        }
    }

}

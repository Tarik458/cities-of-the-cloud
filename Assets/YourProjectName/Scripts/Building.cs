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


    void Start()
    {
        BuildGrid BuildGrid = new BuildGrid(width, height, tileSize, Buildings);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            CameraChange();
        }
    }

    private void CameraChange()
    {
        switch(mainCamera.enabled)
        {
            case true:
                mainCamera.enabled = false;
                buildCamera.enabled = true;
                Time.timeScale = 0;
                break;
            case false:
                mainCamera.enabled = true;
                buildCamera.enabled = false;
                Time.timeScale = 1;
                break;
            default:
                mainCamera.enabled = false;
                buildCamera.enabled = true;
                Time.timeScale = 0;
                break;
        }
    }

}

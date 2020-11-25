using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float StartingDistance;
    public float CameraSpeed;
    public Camera BuildCam;
    public int MinZoom;
    public int MaxZoom;
    public bool camActive = false;
    

    private float cameraX, cameraZ;
    private float changeInX, changeInY, changeInZ;

    private void Start()
    {
        changeInY = StartingDistance;
    }

    void Update()
    {
        Zoom();
        if (camActive)
        {
            Move();
        }
    }

    public void SetActive(bool activate)
    {
        camActive = activate;
    }

    private void Move()
    {
        cameraX = 0;
        cameraZ = 0;
        if(Input.GetKey(KeyCode.D))
        {
            changeInX = 1;
            cameraX += changeInX * CameraSpeed * changeInY;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            changeInX = -1;
            cameraX += changeInX * CameraSpeed * changeInY;
        }

        if (Input.GetKey(KeyCode.W))
        {
            changeInZ = 1;
            cameraZ += changeInZ * CameraSpeed * changeInY;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            changeInZ = -1;
            cameraZ += changeInZ * CameraSpeed * changeInY;
        }

        BuildCam.transform.Translate(cameraX, cameraZ, 0);
    }

    private void Zoom()
    {
        if (Input.mouseScrollDelta.y < 0 && changeInY >= MinZoom && changeInY < MaxZoom)
        {
            changeInY++;
        }
        if (Input.mouseScrollDelta.y > 0 && changeInY <= MaxZoom && changeInY > MinZoom)
        {
            changeInY--;
        }

        if (changeInY >= MinZoom && changeInY <= MaxZoom)
        {
            BuildCam.orthographicSize = changeInY;
        }
    }

}

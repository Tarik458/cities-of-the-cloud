using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class BuildGrid
{
    public int width;
    public int height;
    public BuildingSystem.EBuildings[,] tileArray;
    public GameObject[,] tileObjArray;
    public Vector3[,] positionsArray;
    public bool[,] tileActiveArray;
    public int tileSize;
    public Vector2 gridOrigin;
    public Transform parent;
    string filePath = "/playerCity";

    public BuildGrid(int width, int height, int tileSize, GameObject blankTile, Transform parent)
    {
        this.width = width;
        this.height = height;
        this.tileSize = tileSize;
        this.parent = parent;
        tileArray = new BuildingSystem.EBuildings[width, height];
        tileObjArray = new GameObject[width, height];
        positionsArray = new Vector3[width, height];
        tileActiveArray = new bool[width, height];

        // Minus half to centre grid.
        gridOrigin = new Vector2(-(width/2) * tileSize, -(height/2) * tileSize);

        // Create grid of blank tiles. (Maybe needs to be list so can be expanded more? Or large of blank tiles so building space not limited?) 
        for (int x = 0; x < tileArray.GetLength(0); x++)
        {
            for (int y = 0; y < tileArray.GetLength(1); y++)
            {
                tileArray[x, y] = BuildingSystem.EBuildings.NULL;
                Vector2 worldPos = TranslateToWorldPos(x, y);
                Vector3 worldPos3 = new Vector3(worldPos.x, 1f, worldPos.y);
                positionsArray[x, y] = worldPos3;
                if (x >= 11 && x <= 13 && y >= 11 && y <= 13)
                {
                    tileArray[x, y] = BuildingSystem.EBuildings.STATIONARY_ISLAND;                  
                }
                else
                {
                    GameObject obj = LeanPool.Spawn(blankTile, parent);
                    obj.SetActive(false);
                    obj.transform.position = worldPos3 + parent.transform.position;
                    obj.tag = "BuildTile";
                    tileObjArray[x, y] = obj;

                    if (x >= 10 && x <= 14 && y >= 10 && y <= 14) tileActiveArray[x, y] = true;
                }
            }
        }
    }

    public void saveCity(CitySave_Loading Saver)
    {
        Saver.Save(tileArray, filePath, width);
    }


    // Get the world position to set tile on.
    private Vector2 TranslateToWorldPos(int x, int y)
    {
        Vector2 pos = gridOrigin;
        pos.x += (x * tileSize);
        pos.y += (y * tileSize);
        return pos;
    }

    public Vector2Int TranslateWorldToGridPos(float x, float z)
    {
        //Debug.Log("world pos: " + x + ", " + z);
        Vector2 translatedPos = new Vector2(x, z);
        translatedPos.x -= (parent.position.x + gridOrigin.x - (tileSize * 0.5f));
        translatedPos.y -= (parent.position.z + gridOrigin.y - (tileSize * 0.5f));
        //Debug.Log("translated pos: " + translatedPos);

        Vector2Int pos = Vector2Int.zero;
        pos.x = (int)(translatedPos.x / tileSize);
        pos.y = (int)(translatedPos.y / tileSize);
        //Debug.Log("grid pos: " + pos);
        return pos;
    }
    public Vector2Int getSize() {
        return new Vector2Int(width, height);
    }
    public Vector2Int TranslateWorldToGridPos(Vector3 worldPos)
    {
        return TranslateWorldToGridPos(worldPos.x, worldPos.z);
    }
    public Vector3 getWorldPos(Vector2Int tilePos)
    {
        return positionsArray[tilePos.x, tilePos.y] + parent.transform.position;
    }
    public Vector3 getWorldPos(int x, int y) {
        return positionsArray[x, y] + parent.transform.position;
    }
    public BuildingSystem.EBuildings getTileBuilding(Vector2Int tilePos)
    {
        return tileArray[tilePos.x, tilePos.y];
    }
    public BuildingSystem.EBuildings getTileBuilding(int x, int y) {
        return tileArray[x, y];
    }
    public GameObject getTileObj(Vector2Int tilePos) {
        return tileObjArray[tilePos.x, tilePos.y];
    }
    public GameObject getTileObj(int x, int y) {
        return tileObjArray[x, y];
    }
    public void setTileBuilding(Vector2Int tilePos, BuildingSystem.EBuildings building, GameObject obj) {
        tileArray[tilePos.x, tilePos.y] = building;
        tileObjArray[tilePos.x, tilePos.y] = obj;
    }
    public void setTileActive(int tileposX, int tileposY, bool buildMode, bool active = true)
    {
        tileActiveArray[tileposX, tileposY] = active;
        if(buildMode) tileObjArray[tileposX, tileposY].SetActive(active);
    }
    public void setBlankTilesVisibility(bool visible) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (tileArray[x,y] == BuildingSystem.EBuildings.NULL && tileObjArray[x, y] != null) {
                    if (!visible) tileObjArray[x, y].SetActive(false);
                    else if (tileActiveArray[x, y]) tileObjArray[x, y].SetActive(true);
                }
            }
        }
    }
}

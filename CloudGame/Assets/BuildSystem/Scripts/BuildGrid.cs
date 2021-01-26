using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class BuildGrid
{
    private int width;
    private int height;
    private BuildingSystem.EBuildings[,] tileArray;
    private Vector3[,] positionsArray;    
    private int tileSize;
    private Vector2 gridOrigin;
    private Transform parent;

    public BuildGrid(int width, int height, int tileSize, GameObject blankTile, Transform parent)
    {
        this.width = width;
        this.height = height;
        this.tileSize = tileSize;
        this.parent = parent;
        tileArray = new BuildingSystem.EBuildings[width, height];
        positionsArray = new Vector3[width, height];

        // Minus half to centre grid.
        gridOrigin = new Vector2(-(width/2) * tileSize, -(height/2) * tileSize);

        // Create grid of blank tiles. (Maybe needs to be list so can be expanded more? Or large of blank tiles so building space not limited?) 
        for (int x = 0; x < tileArray.GetLength(0); x++)
        {
            for (int y = 0; y < tileArray.GetLength(1); y++)
            {
                tileArray[x, y] = BuildingSystem.EBuildings.NULL;
                Vector2 worldPos = TranslateToWorldPos(x, y);
                Vector3 worldPos3 = new Vector3(worldPos.x, 0f, worldPos.y);
                positionsArray[x, y] = worldPos3;
                if (x == 12 && y == 12)
                {
                    tileArray[x, y] = BuildingSystem.EBuildings.STATIONARY_ISLAND;
                    GameObject obj = LeanPool.Spawn(blankTile, parent);
                    obj.SetActive(true);
                    obj.transform.position = worldPos3 + parent.transform.position;
                    obj.tag = "BuildTile";
                }
                else
                {
                    GameObject obj = LeanPool.Spawn(blankTile, parent);
                    obj.SetActive(true);
                    obj.transform.position = worldPos3 + parent.transform.position;
                    obj.tag = "BuildTile";
                }
            }
        }
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
    public Vector2Int TranslateWorldToGridPos(Vector3 worldPos)
    {
        return TranslateWorldToGridPos(worldPos.x, worldPos.z);
    }

    public Vector3 getWorldPos(Vector2Int tilePos)
    {
        return positionsArray[tilePos.x, tilePos.y] + parent.transform.position;
    }
    public BuildingSystem.EBuildings getTileBuilding(Vector2Int tilePos)
    {
        return tileArray[tilePos.x, tilePos.y];
    }
    public void setTileBuilding(Vector2Int tilePos, BuildingSystem.EBuildings building)
    {
        tileArray[tilePos.x, tilePos.y] = building;
    }
}

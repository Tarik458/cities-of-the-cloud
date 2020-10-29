using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGrid
{
    private int width;
    private int height;
    private int[,] tileArray;
    private int tileSize;
    private List<GameObject> Buildings;

    public BuildGrid(int width, int height, int tileSize, List<GameObject> Buildings)
    {
        this.Buildings = Buildings;
        this.width = width;
        this.height = height;
        tileArray = new int[width, height];

        for (int x = 0; x < tileArray.GetLength(0); x++)
        {
            for (int y = 0; y < tileArray.GetLength(1); y++)
            {
                
                Object.Instantiate(Buildings[0], new Vector3(TranslateToWorldPos(x, y)[0], 0f, TranslateToWorldPos(x, y)[1]), Quaternion.identity);
            }
        }
    }

        

    private float[] TranslateToWorldPos(int x, int y)
    {
        float[] xzPos = new float[2];
        Vector3 worldPos = new Vector3(x, y) * tileSize;

        // Sets for right axes
        worldPos.x -= 0.5f * tileSize;
        worldPos.z = worldPos.y - 0.5f * tileSize;
        worldPos.y = 0;

        xzPos[0] = worldPos.x;
        xzPos[1] = worldPos.z;
        return xzPos;

    }
}

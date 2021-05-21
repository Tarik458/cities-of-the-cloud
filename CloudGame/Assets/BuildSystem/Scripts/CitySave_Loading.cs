using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class CitySave_Loading
{
    XmlSerializer xmlSerializer;

    string file;
    string extension;

    public CitySave_Loading()
    {
        file = "/playerCity";
        extension = ".xml";
    }

    string fullPathSave()
    {
        return Application.persistentDataPath + file + extension;
    }

    string fullPathInitialLoad()
    {
        return "Assets/Resources/basePlayerCity" + extension;
    }
    
    void setPath(string newPath)
    {
        file = newPath;
    }

    public void Save(BuildingSystem.EBuildings[,] cityArray, string fileName, int width_Height)
    {
        xmlSerializer = null;
        setPath(fileName);
        xmlSerializer = new XmlSerializer(typeof(BuildingSystem.EBuildings[]));
        FileStream writeStream = new FileStream(fullPathSave(), FileMode.OpenOrCreate);
        BuildingSystem.EBuildings[] cityArray1D = new BuildingSystem.EBuildings[width_Height * width_Height];

        int counter1D = 0;
        for (int x = 0; x < width_Height; x++)
        {
            for (int y = 0; y < width_Height; y++)
            {
                cityArray1D[counter1D] = cityArray[x, y];
                counter1D++;
            }
        }
        writeStream.SetLength(0);
        xmlSerializer.Serialize(writeStream, cityArray1D);

        writeStream.Close();
        Debug.Log("File saved to: " + fullPathSave());
    }

    public bool checkFile(string fileName)
    {
        setPath(fileName);
        if (File.Exists(fullPathSave()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public BuildingSystem.EBuildings[,] Load(string fileName, int width_Height, bool first = false)
    {
        xmlSerializer = null;
        setPath(fileName);
        xmlSerializer = new XmlSerializer(typeof(BuildingSystem.EBuildings[]));

        FileStream readStream;

        if (first)
        {
            readStream = new FileStream(fullPathInitialLoad(), FileMode.Open);
        }
        else
        {
            readStream = new FileStream(fullPathSave(), FileMode.Open);
        }

        BuildingSystem.EBuildings[,] loadedCity = new BuildingSystem.EBuildings[width_Height, width_Height];

        BuildingSystem.EBuildings[] loadedCity1D = (BuildingSystem.EBuildings[])xmlSerializer.Deserialize(readStream);

        int counter1D = 0;
        for (int x = 0; x < width_Height; x++)
        {
            for (int y = 0; y < width_Height; y++)
            {
                loadedCity[x, y] = loadedCity1D[counter1D];
                counter1D++;
            }
        }

        Debug.Log("File loaded from: " + fullPathSave());
        readStream.Close();
        return loadedCity;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButton : MonoBehaviour
{
    public BuildingSystem.EBuildings m_building;
    static BuildingSystem s_buildingSystem;
    public static void setBuildingSystem(BuildingSystem obj)
    {
        s_buildingSystem = obj;
    }
    public void select() 
    {
        s_buildingSystem.SelectBuildingToPlace(m_building);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager
{
    int m_food;
    int m_people;
    int m_materials;
    int m_foodMax;
    int m_peopleMax;
    int m_materialsMax;
    Text m_foodText;
    Text m_peopleText;
    Text m_materialsText;

    public ResourceManager(int food = 500, int people = 100, int materials = 100)
    {
        m_food = food;
        m_people = people;
        m_materials = materials;
    }

    void Update()
    {
        updateUI();
    }

    void updateUI() {
        m_foodText.text = "Food: " + m_food.ToString();
        m_peopleText.text = "People: " + m_people.ToString();
        m_materialsText.text = "Materials: " + m_materials.ToString();
    }
    public void SetUIObjs(Text food, Text people, Text materials)
    {
        m_foodText = food;
        m_peopleText = people;
        m_materialsText = materials;
        updateUI();
    }

    public int GetResourceValue(string resourceType)
    {
        string typeCheck = resourceType;
        int getVal;
        switch (typeCheck)
        {
            case "food":
                getVal = m_food;
                break;
            case "people":
                getVal = m_people;
                break;
            case "materials":
                getVal = m_materials;
                break;
            default:
                Debug.Log("a whadda ya doin?");
                getVal = 0;
                break;

        }
        //Debug.Log("get val " + getVal);

        return getVal;
    }

    public bool SetResourceValue(string resourceType, int changeValue)
    {       
        bool validChange = false;
        ref int resourceVal = ref m_food;
        int clamp = 0;
        switch (resourceType) {
            case "food": 
                resourceVal = ref m_food;
                clamp = m_foodMax;
                break;
            case "people":
                resourceVal = ref m_people;
                clamp = m_peopleMax;
                break;
            case "materials":
                resourceVal = ref m_materials;
                clamp = m_materialsMax;
                break;
            default:
                Debug.LogError("Invalid resource type string");                
                break;
        }

        if(changeValue < 0) {
            if(Mathf.Abs(changeValue) <= resourceVal) {
                resourceVal += changeValue;
                validChange = true;
            }
        }
        else {
            resourceVal += changeValue;
            resourceVal = Mathf.Clamp(resourceVal, 0, clamp);
            validChange = true;
        }

        if (validChange) updateUI();
        return validChange;
    }

    // Add resources gained from buildings & combat.
    public void TickUp(BuildingSystem bldgrid /* ,int enemySize*/)
    {
        int foodProd = 0;
        int materialsProd = 0;
        int peopleProd = 0;

        Vector2Int checkTile;


        // Calculate number of buildings producing specific resource.
        for (int x = 0; x < bldgrid.getBuildGrid().getSize().x; x++)
        {
            for (int y = 0; y < bldgrid.getBuildGrid().getSize().y; y++)
            {
                checkTile = new Vector2Int(x, y);

                switch (bldgrid.getBuildGrid().getTileBuilding(checkTile))
                {
                    case BuildingSystem.EBuildings.BUILDING_FARM:
                        materialsProd += 1;
                        break;
                    case BuildingSystem.EBuildings.BUILDING_DIVINGSTATION:
                        materialsProd += 1;
                        break;
                    case BuildingSystem.EBuildings.BUILDING_HUT:
                        foodProd += 1;
                        break;
                    case BuildingSystem.EBuildings.BUILDING_WORKSHOP:
                        peopleProd += 1;
                        break;
                    default:
                        break;
                }
            }
        }

        SetResourceValue("food", /*building resource increase*/ 5 * foodProd);
        SetResourceValue("materials", /*building resource increase*/ 5 * materialsProd);
        SetResourceValue("people", /*building resource increase*/ 5 * peopleProd);

        // Rewards from (enemy city * enemySize) or beast encounter.
    }


}

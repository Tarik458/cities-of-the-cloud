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

    public ResourceManager(int food = 500, int people = 200, int materials = 300)
    {
        if (StaticVals.FirstTime)
        {
            m_food = food;
            m_people = people;
            m_materials = materials;
        }
        else
        {
            m_food = StaticVals.Food ;
            m_people = StaticVals.Citizens;
            m_materials = StaticVals.Materials;
        }

        m_foodMax = 9999;
        m_peopleMax = 9999;
        m_materialsMax = 9999;
    }

    void Update()
    {
        updateUI();
    }

    void updateUI() {
        m_foodText.text = "Food: " + m_food.ToString();
        m_peopleText.text = "People: " + m_people.ToString();
        m_materialsText.text = "Materials: " + m_materials.ToString();

        StaticVals.Food = m_food;
        StaticVals.Citizens = m_people;
        StaticVals.Materials = m_materials;
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
    public void TickUp(BuildingSystem bldgrid /* ,int enemySize, depth for diving room*/)
    {
        // Initilised value including island resources.
        int foodProd = 10;
        int materialProd = 5;
        int peopleProd = 0;

        Vector2Int checkTile;

        //affected by damage?

        // Calculate number of buildings producing specific resource.
        for (int x = 0; x < bldgrid.getBuildGrid().getSize().x; x++)
        {
            for (int y = 0; y < bldgrid.getBuildGrid().getSize().y; y++)
            {
                checkTile = new Vector2Int(x, y);

                switch (bldgrid.getBuildGrid().getTileBuilding(checkTile))
                {
                    case BuildingSystem.EBuildings.BUILDING_HUT:
                        // + people? +25 max pop.(1 off or recalculate), or is maxpop currpop
                        foodProd += 1;
                        break; 
                    case BuildingSystem.EBuildings.BUILDING_FARM:
                        foodProd += 50;
                        // -2 tools?
                        break;
                    case BuildingSystem.EBuildings.BUILDING_WORKSHOP:
                        materialProd += 10;
                        // +15 tools?
                        // +5 loot from enemy city
                        break;
                    case BuildingSystem.EBuildings.BUILDING_DIVINGSTATION:
                        //if depth >500m -> default: +10mat +5food: *ground mats multiplier.
                        materialProd += 10; //*modifier
                        foodProd += 5; //*modifier
                        break;
                    case BuildingSystem.EBuildings.BUILDING_REFINERY:
                        // +5 explosives, -5 tools
                        materialProd -= 2;
                        break;
                    case BuildingSystem.EBuildings.BUILDING_GUARDTOWER:
                        // - 5 tools
                        break;
                    case BuildingSystem.EBuildings.BUILDING_TREBUCHET:
                        materialProd -= 10;
                        // explosive shots?
                        break;
                    default:
                        break;
                }
            }
        }

        SetResourceValue("food", /*building resource increase*/ 5 * foodProd);
        SetResourceValue("materials", /*building resource increase*/ 5 * materialProd);
        SetResourceValue("people", /*building resource increase*/ 5 * peopleProd);

        // Rewards from (enemy city * enemySize) or beast encounter: city(+materials & +people), beast(+food & +materials)
    }


}

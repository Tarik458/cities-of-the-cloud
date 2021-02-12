using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager
{
    int m_food;
    int m_people;
    int m_materials;
    int m_foodMax = 100;
    int m_peopleMax = 100;
    int m_materialsMax = 100;
    Text m_foodText;
    Text m_peopleText;
    Text m_materialsText;

    public ResourceManager(int food = 100, int people = 100, int materials = 100)
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
        Debug.Log("get val " + getVal);

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
}

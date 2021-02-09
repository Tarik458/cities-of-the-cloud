using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public int food;
    public int people;
    public int materials;
    public Text foodtxt;
    public Text peopletxt;
    public Text materialstxt;

    // Start is called before the first frame update
    void Start()
    {
        food = 100;
        people = 100;
        materials = 100;
    }

    // Update is called once per frame
    void Update()
    {
        SetUIVals();
    }

    private void SetUIVals()
    {
        foodtxt.text = "Food: " + food.ToString();
        peopletxt.text = "People: " + people.ToString();
        materialstxt.text = "Materials: " + materials.ToString();
    }

    public int GetResourceValue(string resourceType)
    {
        string typeCheck = resourceType;
        int getVal;
        switch (typeCheck)
        {
            case "food":
                getVal = food;
                break;
            case "people":
                getVal = people;
                break;
            case "materials":
                getVal = materials;
                break;
            default:
                Debug.Log("a whadda ya doin?");
                getVal = 0;
                break;

        }

        return getVal;
    }

    public bool SetResourceValue(string resourceType, int changeValue)
    {
        string typeCheck = resourceType;
        int sumValue;
        int de_signedChange;
        bool validChange;

        if (changeValue < 0)
        {
            de_signedChange = changeValue * -1;
        }
        else
        {
            de_signedChange = changeValue;
        }
        
        switch (typeCheck)
        {
            case "food":
                if (de_signedChange < food)
                {
                    sumValue = food + changeValue;
                    validChange = true;
                }
                validChange = false;
                break;
            case "people":
                if (de_signedChange < people)
                {
                    sumValue = people + changeValue;
                    validChange = true;
                }
                validChange = false;
                break;
            case "materials":
                if (de_signedChange < materials)
                {
                    sumValue = materials + changeValue;
                    validChange = true;
                }
                validChange = false;
                break;
            default:
                Debug.Log("a whadda ya doin?");
                sumValue = -1;
                validChange = false;
                break;
        }

        return validChange;

    }

}

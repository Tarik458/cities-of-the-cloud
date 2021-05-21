using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVals
{
    private static bool firstTime = true;
    private static int food, materials, citizens;

    public static bool FirstTime
    {
        get
        {
            return firstTime;
        }
        set
        {
            firstTime = value;
        }
    }

    public static int Food
    {
        get
        {
            return food;
        }
        set
        {
            food = value;
        }
    }

    public static int Materials
    {
        get
        {
            return materials;
        }
        set
        {
            materials = value;
        }
    }

    public static int Citizens
    {
        get
        {
            return citizens;
        }
        set
        {
            citizens = value;
        }
    }
}

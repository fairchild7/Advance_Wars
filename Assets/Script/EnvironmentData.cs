using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentData
{
    private static EnvironmentData instance;
    public static EnvironmentData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnvironmentData();
            }
            return instance;
        }
    }

    //Defense value of environments
    public int[] defenseValue = new int[] { 1, 0, 2, 0, 3, 4 };
}


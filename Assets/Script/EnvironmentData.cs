using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentData
{
    //Defense value of environments
    public int[] defenseValue = new int[] { 1, 0, 2, 0, 3, 4 };

    /*Passability of environments and units, 
    if unit has passability less than environments, it means unit can move through environment
    if equal, it means unit can move but slowly
    if more, it means unit can't move
    */
    public int[] environmentPassability = new int[] { 3, 3, 3, 1, 1, 3 };
}


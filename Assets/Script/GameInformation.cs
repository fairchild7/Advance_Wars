using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInformation
{
    //Defense value of environments
    public int[] defenseValue = new int[] { 0, 0, 1, 2, 3, 4 };

    //Damage value of units to units, for example, if an Infantry attacks an Infantry, damage will be damageValue[0,0] = 55%
    public int[,] damageValue = new int[,] { { 55, 45, 5, 15, 12}, 
                                            { 65, 55, 6, 32, 18},
                                            { 75, 70, 55, 70, 85}, 
                                            { 90, 85, 70, 75, 80}, 
                                            { 70, 65, 6, 45, 35} };

    //Move range of units
    public int[] moveRange = new int[] { 3, 2, 6, 5, 8 };

    /*Passability of environments and units, 
    if unit has passability less than environments, it means unit can move through environment
    if equal, it means unit can move but slowly
    if more, it means unit can't move
    */
    public int[] environmentPassability = new int[] { 3, 1, 3, 3, 1, 3 };
    public int[] unitPassability = new int[] { 1, 0, 2, 2, 2 };

    
    public enum Environment
    {
        road,
        river,
        grass,
        tree,
        mountain,
        building,
    }
    public enum Unit
    {
        Infantry,
        Artillery,
        Small_Tank,
        Cannon,
        Recon,
    }

}

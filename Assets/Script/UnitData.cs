using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData
{
    private Dictionary<string, UnitType> objectToUnit = new Dictionary<string, UnitType>
    {
        { "Infantry", UnitType.Infantry },
        { "Artillery", UnitType.Artillery },
        { "Small_Tank", UnitType.Small_Tank },
        { "Cannon", UnitType.Cannon },
        { "Recon", UnitType.Recon }
    };

    private Dictionary<string, UnitColor> objectToColor = new Dictionary<string, UnitColor>
    {
        { "RedUnit", UnitColor.Red },
        { "BlueUnit", UnitColor.Blue }
    };

    //Damage value of units to units, for example, if an Infantry attacks an Infantry, damage will be damageValue[0,0] = 55%
    public int[,] damageValue = new int[,] { { 55, 45, 5, 15, 12}, 
                                            { 65, 55, 55, 70, 85},
                                            { 75, 70, 55, 70, 85}, 
                                            { 90, 85, 70, 75, 80}, 
                                            { 70, 65, 6, 45, 35} };

    public int[] attackRange = new int[] { 1, 1, 1, 3, 1 };
    //Move range of units
    public int[] moveRange = new int[] { 3, 2, 6, 5, 8 };

    /*Passability of environments and units, 
    if unit has passability less than environments, it means unit can move through environment
    if equal, it means unit can move but slowly
    if more, it means unit can't move
    */
    public int[] unitPassability = new int[] { 1, 0, 2, 2, 2 };

    public enum UnitType
    {
        Infantry,
        Artillery,
        Small_Tank,
        Cannon,
        Recon,
    }
    public enum UnitColor
    {
        Red,
        Blue,
        Neutral,
    }

    public UnitType GetUnitTypeFromName(string objectName)
    {
        if (objectToUnit.TryGetValue(objectName, out UnitType unitType))
        {
            return unitType;
        }
        return UnitType.Infantry; //return Infantry if not find, notice this
    }

    public UnitColor GetUnitColor(string objectLayer)
    {
        if (objectToColor.TryGetValue(objectLayer, out UnitColor unitColor))
        {
            return unitColor;
        }
        return UnitColor.Neutral;
    }
}
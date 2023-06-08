using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string Name;
    public int moveDistance;
    public int attackRange;
    public int unitPassability;
    public bool isMoved = false;
    public bool isSelected = false;
    SpriteRenderer spriteHp;
    GameInformation gameInfo;
    public GameInformation.UnitType unitType;

    private void Awake()
    {
        gameInfo = new GameInformation();
        GetUnitType();
        moveDistance = GetUnitMoveRange();
        attackRange = GetUnitAttackRange();
        unitPassability = GetUnitPassability();
        spriteHp = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteHp.gameObject.SetActive(false);
    }

    public void GetUnitType()
    {
        string type = gameObject.tag;
        unitType = gameInfo.GetUnitTypeFromName(type);
    }

    private int GetUnitMoveRange()
    {
        return gameInfo.moveRange[(int)unitType];
    }

    private int GetUnitAttackRange()
    {
        return gameInfo.attackRange[(int)unitType];
    }

    private int GetUnitPassability()
    {
        return gameInfo.unitPassability[(int)unitType];
    }
}

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

    public Color originalColor;
    SpriteRenderer spriteHp;
    UnitData gameInfo;
    public UnitData.UnitType unitType;
    public UnitData.UnitColor unitColor;

    private void Awake()
    {
        gameInfo = new UnitData();
        GetUnitType();
        GetUnitColor();
        moveDistance = GetUnitMoveRange();
        attackRange = GetUnitAttackRange();
        unitPassability = GetUnitPassability();

        spriteHp = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteHp.gameObject.SetActive(false);
        originalColor = GetComponent<SpriteRenderer>().color;
    }

    public void GetUnitType()
    {
        string type = gameObject.tag;
        unitType = gameInfo.GetUnitTypeFromName(type);
    }

    public void GetUnitColor()
    {
        string layer = LayerMask.LayerToName(gameObject.layer);
        unitColor = gameInfo.GetUnitColor(layer);
    }

    public void ActionWait()
    {
        if (isMoved)
        {
            this.GetComponent<SpriteRenderer>().color = Color.gray;
        }
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

    public Vector2Int GetUnitPos()
    {
        return this.GetComponent<MapElement>().GetUnitPos();
    }
}

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
    GameInformation gameInfo;
    public GameInformation.UnitType unitType;
    public GameInformation.UnitColor unitColor;

    private void Awake()
    {
        gameInfo = new GameInformation();
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

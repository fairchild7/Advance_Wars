using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private float hp;

    public string Name;
    public int moveDistance;
    public int attackRange;
    public bool isMoved = false;
    public bool isSelected = false;

    public Color originalColor;
    SpriteRenderer spriteHp;
    public UnitData.UnitType unitType;
    private UnitData.UnitColor unitColor;

    private void Awake()
    {
        GetUnitType();
        SetUnitColor();
        moveDistance = GetUnitMoveRange();
        attackRange = GetUnitAttackRange();
        hp = 100f;

        spriteHp = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteHp.gameObject.SetActive(false);
        originalColor = GetComponent<SpriteRenderer>().color;
    }

    public float GetHp()
    {
        return hp;
    }

    public void GetDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0f)
        {
            RemoveUnitFromPlay();
            return;
        }
        if (hp >= 100f)
        {
            hp = 100f;
            spriteHp.gameObject.SetActive(false);
            return;
        }
        if (Mathf.RoundToInt(hp / 10f) > 0 && Mathf.RoundToInt(hp / 10f) < 10)
        {
            spriteHp.gameObject.SetActive(true);
            spriteHp.sprite = GameController.Instance.hpSprites[Mathf.RoundToInt(hp / 10f) - 1];
        }
        if (Mathf.RoundToInt(hp / 10f) == 0 && hp > 0f)
        {
            spriteHp.gameObject.SetActive(true);
            spriteHp.sprite = GameController.Instance.hpSprites[0];
        }
    }

    public UnitData.UnitColor GetUnitColor()
    {
        return unitColor;
    }

    public void GetUnitType()
    {
        string type = gameObject.tag;
        unitType = UnitData.Instance.GetUnitTypeFromName(type);
    }

    private void SetUnitColor()
    {
        string layer = LayerMask.LayerToName(gameObject.layer);
        unitColor = UnitData.Instance.GetUnitColor(layer);
    }

    private int GetUnitMoveRange()
    {
        return UnitData.Instance.moveRange[(int)unitType];
    }

    private int GetUnitAttackRange()
    {
        return UnitData.Instance.attackRange[(int)unitType];
    }

    public Vector2Int GetUnitPos()
    {
        return this.GetComponent<MapElement>().GetUnitPos();
    }

    public void RemoveUnitFromPlay()
    {
        GetComponent<MapElement>().RemoveObjectFromGrid();
        gameObject.SetActive(false);
        if (unitColor == UnitData.UnitColor.Red)
        {
            GameController.Instance.allyUnit.Remove(this);
            if (GameController.Instance.allyUnit.Count == 0)
            {
                GameController.Instance.loseCondition = true;
                GameController.Instance.CheckCondition();
            }
        }
        else if (unitColor == UnitData.UnitColor.Blue)
        {
            GameController.Instance.enemyUnit.Remove(this);
            GameController.Instance.enemyDestroyed += 1;
            if (GameController.Instance.enemyUnit.Count == 0)
            {
                GameController.Instance.winCondition = true;
                GameController.Instance.CheckCondition();
            }
        }
    }
}

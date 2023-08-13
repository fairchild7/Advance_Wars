using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    HQ,
    City,
    Workshop,
    Shipyard,
    Airport,
}

public class Building : MonoBehaviour
{
    private UnitData.UnitColor color;
    [SerializeField] BuildingType type;
    private int hp;

    private void Awake()
    {
        hp = 20;
        color = UnitData.Instance.GetUnitColor(LayerMask.LayerToName(gameObject.layer));
        SetType();
        switch (color)
        {
            case UnitData.UnitColor.Red:
                {
                    GameController.Instance.allyBuilding.Add(this);
                    return;
                }
            case UnitData.UnitColor.Blue:
                {
                    GameController.Instance.enemyBuilding.Add(this);
                    return;
                }
            case UnitData.UnitColor.Neutral:
                {
                    GameController.Instance.neutralBuilding.Add(this);
                    return;
                }
        }
    }

    public UnitData.UnitColor GetColor()
    {
        return color;
    }

    public int GetHp()
    {
        return hp;
    }

    public void GetDamage(int damage)
    {
        hp -= damage;
    }

    public void ChangeColor(UnitData.UnitColor color)
    {
        if (type == BuildingType.HQ)
        {
            switch (this.color)
            {
                case UnitData.UnitColor.Red:
                    {
                        GameController.Instance.loseCondition = true;
                        GameController.Instance.CheckCondition();
                        return;
                    }
                case UnitData.UnitColor.Blue:
                    {
                        GameController.Instance.allyBuilding.Add(this);
                        GameController.Instance.winCondition = true;
                        GameController.Instance.CheckCondition();
                        return;
                    }
            }
        }
        Building newBuilding = Instantiate(Resources.Load(color.ToString() + "City"), transform.position, Quaternion.identity, transform.parent) as Building;
        switch (this.color)
        {
            case UnitData.UnitColor.Red:
                {
                    GameController.Instance.allyBuilding.Remove(this);
                    break;
                }
            case UnitData.UnitColor.Blue:
                {
                    GameController.Instance.enemyBuilding.Remove(this);
                    break;
                }
            case UnitData.UnitColor.Neutral:
                {
                    GameController.Instance.neutralBuilding.Remove(this);
                    break;
                }
        }
        Destroy(gameObject);
    }

    private void SetType()
    {
        if (gameObject.CompareTag("City"))
        {
            type = BuildingType.City;
        }
        else if (gameObject.CompareTag("HQ"))
        {
            type = BuildingType.HQ;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanel : MonoBehaviour
{
    [SerializeField] Image unitSprite;
    [SerializeField] TextMeshProUGUI unitName;
    [SerializeField] TextMeshProUGUI unitMoveRange;
    [SerializeField] TextMeshProUGUI unitAttackRange;

    public void UpdateUnitPanel(Unit unit)
    {
        unitSprite.sprite = unit.GetComponent<SpriteRenderer>().sprite;
        unitName.text = unit.unitType.ToString();
        unitMoveRange.text = "Move range: " + unit.moveDistance;
        unitAttackRange.text = "Attack range: " + unit.attackRange;
    }
}

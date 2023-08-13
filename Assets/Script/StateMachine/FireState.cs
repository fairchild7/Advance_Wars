using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireState : IState
{
    Vector3Int clickPosition;

    public void OnEnter()
    {
        
    }

    public void OnExecute()
    {
        MouseInput();
    }

    public void OnExit()
    {

    }

    public void MouseInput()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPosition = UnitControl.Instance.targetTilemap.WorldToCell(worldPoint);

        if (Input.GetMouseButton(0))
        {
            if (!UnitControl.Instance.CheckClickPos(clickPosition.x, clickPosition.y))
            {
                return;
            }
            UIGamePlay.Instance.ShowEnvironmentPanel(clickPosition.x, clickPosition.y);
            UnitControl.Instance.SelectUnitAtClickPos(clickPosition.x, clickPosition.y);
            if (UnitControl.Instance.selectedUnit != null)
            {
                UIGamePlay.Instance.ShowUnitPanel(UnitControl.Instance.selectedUnit);
            }
            else
            {
                UIGamePlay.Instance.HideUnitPanel();
            }

            List<Unit> attackableEnemy = UnitControl.Instance.GetEnemyInRange(UnitControl.Instance.GetAttackList(UnitControl.Instance.currentUnit, 
                UnitControl.Instance.currentUnit.GetUnitPos()), UnitControl.Instance.currentUnit);

            foreach (Unit enemy in attackableEnemy)
            {
                Vector2Int enemyPos = enemy.GetUnitPos();
                if (new Vector2Int(clickPosition.x, clickPosition.y) == enemyPos)
                {
                    UnitControl.Instance.Attack(UnitControl.Instance.currentUnit, enemy);
                    UnitControl.Instance.ClearHighlightMap();
                    UnitControl.Instance.ActionWait();
                    return;
                }
            }  
        }
    }
}

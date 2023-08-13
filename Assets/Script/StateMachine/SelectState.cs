using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectState : IState
{
    Vector3Int clickPosition;

    public void OnEnter()
    {
        UnitControl.Instance.currentUnit = UnitControl.Instance.selectedUnit;
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

        if (Input.GetMouseButtonDown(0))
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
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeState : IState
{
    Vector3Int clickPosition;

    public void OnEnter()
    {
        UIGamePlay.Instance.HideAllButtons();
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

            UnitControl.Instance.ClearHighlightMap();

            UIGamePlay.Instance.ShowEnvironmentPanel(clickPosition.x, clickPosition.y);
            //Case 1: Nothing is selected
            if (UnitControl.Instance.selectedUnit == null)
            {
                //Try selecting unit at click position
                UnitControl.Instance.SelectUnitAtClickPos(clickPosition.x, clickPosition.y);
                //Case 1.1: There is an unit being selected after click
                if (UnitControl.Instance.selectedUnit != null)
                {
                    UIGamePlay.Instance.HideAllButtons();
                    UIGamePlay.Instance.ShowUnitPanel(UnitControl.Instance.selectedUnit);
                    //Case 1.1.1: If moved,
                    if (UnitControl.Instance.CheckAllyUnit(UnitControl.Instance.selectedUnit))
                    {
                        if (UnitControl.Instance.selectedUnit.isMoved)
                        {

                        }
                        //Case 1.1.2: If not moved, draw move range
                        else
                        {
                            UnitControl.Instance.currentUnitPos = UnitControl.Instance.selectedUnit.transform.position;
                            UnitControl.Instance.currentUnit = UnitControl.Instance.selectedUnit;
                            UnitControl.Instance.CheckBuilding(UnitControl.Instance.selectedUnit.transform.position);
                            UnitControl.Instance.CheckEnemy(UnitControl.Instance.selectedUnit, UnitControl.Instance.selectedUnit.GetUnitPos());

                            UnitControl.Instance.DrawMoveRange(clickPosition.x, clickPosition.y);
                            UIGamePlay.Instance.buttonWait.SetActive(true);
                        }
                    }
                }
                //Case 1.2: Nothing is selected after click
                else if (UnitControl.Instance.selectedUnit == null)
                {
                    UIGamePlay.Instance.HideUnitPanel();
                }
            }
            //Case 2: There is an unit being selected
            else if (UnitControl.Instance.selectedUnit != null)
            {
                //Case 2.1: There is new unit at clickPos, select it
                if (GridManager.Instance.GetUnit(clickPosition.x, clickPosition.y) != null)
                {
                    UnitControl.Instance.SelectUnitAtClickPos(clickPosition.x, clickPosition.y);
                    UIGamePlay.Instance.HideAllButtons();
                    UIGamePlay.Instance.ShowUnitPanel(UnitControl.Instance.selectedUnit);
                    if (UnitControl.Instance.CheckAllyUnit(UnitControl.Instance.selectedUnit))
                    {
                        //Case 2.1.1: If new unit moved
                        if (UnitControl.Instance.selectedUnit.isMoved)
                        {

                        }
                        //Case 2.1.2: If not moved, draw move range
                        else
                        {
                            UnitControl.Instance.currentUnitPos = UnitControl.Instance.selectedUnit.transform.position;
                            UnitControl.Instance.currentUnit = UnitControl.Instance.selectedUnit;
                            UnitControl.Instance.CheckBuilding(UnitControl.Instance.selectedUnit.transform.position);
                            UnitControl.Instance.CheckEnemy(UnitControl.Instance.selectedUnit, UnitControl.Instance.selectedUnit.GetUnitPos());

                            UnitControl.Instance.DrawMoveRange(clickPosition.x, clickPosition.y);
                            UIGamePlay.Instance.buttonWait.SetActive(true);
                        }
                    }
                }
                //Case 2.2: No unit at clickPos
                else
                {
                    //Case 2.2.1: Unit selected moved
                    if (UnitControl.Instance.selectedUnit.isMoved)
                    {
                        UIGamePlay.Instance.HideUnitPanel();
                    }
                    //Case 2.2.2: Unit selected is not moved, move it
                    else
                    {
                        UnitControl.Instance.MoveOnClick(clickPosition.x, clickPosition.y);

                        if (UnitControl.Instance.selectedUnit != null)
                        {
                            UIGamePlay.Instance.buttonWait.SetActive(true);

                            UnitControl.Instance.CheckBuilding(new Vector2(clickPosition.x + 0.5f, clickPosition.y + 0.5f));
                            UnitControl.Instance.CheckEnemy(UnitControl.Instance.selectedUnit, new Vector2Int(clickPosition.x, clickPosition.y));

                            UIGamePlay.Instance.buttonCancel.SetActive(true);
                            GameController.Instance.ChangeState(new SelectState());
                        }
                        else
                        {
                            UIGamePlay.Instance.HideAllButtons();
                        }
                    }
                }
            }
        }
    }
}

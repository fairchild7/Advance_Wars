using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitControl : MonoBehaviour
{
    [SerializeField] Tilemap targetTilemap;
    [SerializeField] Tilemap highlightTilemap;
    [SerializeField] GameObject highlightPrefab;
    [SerializeField] GameObject attackPrefab;
    [SerializeField] GridManager gridManager;
    [SerializeField] GameObject buttonFire;
    [SerializeField] GameObject buttonWait;
    [SerializeField] GameObject buttonCancel;
 
    PathFinding pathFinding;
    Unit selectedUnit;
    Vector3Int clickPosition;
    
    private void Awake()
    {
        pathFinding = targetTilemap.GetComponent<PathFinding>();    
    }

    private void Update()
    {
        MouseInput();
    }

    /*
    private void MouseInput()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPosition = targetTilemap.WorldToCell(worldPoint);
        if (Input.GetMouseButtonDown(1))
        {
            highlightTilemap.ClearAllTiles();
            if (gridManager.CheckPosition(clickPosition.x, clickPosition.y) == false)
            {
                return;
            }
            selectedUnit = gridManager.GetUnit(clickPosition.x, clickPosition.y);

            if (selectedUnit != null)
            {
                List<PathNode> toHighlight = new List<PathNode>();
                pathFinding.Clear();
                pathFinding.CalculateWalkableTerrain(clickPosition.x, clickPosition.y, selectedUnit.moveDistance, ref toHighlight);

                for (int i = 0; i < toHighlight.Count; i++)
                {
                    highlightTilemap.SetTile(new Vector3Int(toHighlight[i].xPos, toHighlight[i].yPos, 0), highlightTile);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left " + clickPosition.x + " " + clickPosition.y);
            if (selectedUnit == null)
            {
                return;
            }
            highlightTilemap.ClearAllTiles();

            Debug.Log(selectedUnit.unitType);
            List<PathNode> path = pathFinding.TrackBackPath(selectedUnit, clickPosition.x, clickPosition.y);

            if (path != null)
            {
                path.Reverse();
                if (path.Count > 0)
                {
                    for (int i = 0; i < path.Count; i++)
                    {
                        highlightTilemap.SetTile(new Vector3Int(path[i].xPos, path[i].yPos, 0), highlightTile);
                        selectedUnit.GetComponent<MapElement>().MoveUnit(path[i].xPos, path[i].yPos);
                    }          
                }
                Deselect();
            }
            highlightTilemap.ClearAllTiles();
        }
    }
    */

    public void ActionWait()
    {
        selectedUnit.isMoved = true;
        selectedUnit.ActionWait();
    }

    private void MouseInput()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPosition = targetTilemap.WorldToCell(worldPoint);
        
        if (Input.GetMouseButtonDown(0))
        {
            //Case 1: Nothing is selected
            if (selectedUnit == null)
            {
                //Try selecting unit at click position
                SelectUnitAtClickPos();
                //Case 1.1: There is an unit being selected after click
                if (selectedUnit != null)
                {
                    //Case 1.1.1: If moved,
                    if (selectedUnit.isMoved)
                    {
                        Debug.Log(selectedUnit.name + " moved");
                    }
                    //Case 1.1.2: If not moved, draw move range
                    else
                    {
                        DrawMoveRange();
                    }
                }
                //Case 1.2: Nothing is selected after click
                else if (selectedUnit == null)
                {
                    if (gridManager.CheckPosition(clickPosition.x, clickPosition.y) == false)
                    {
                        return;
                    }
                    else
                    {
                        Debug.Log(gridManager.GetTerrainType(clickPosition.x, clickPosition.y));
                    }
                }
            }
            //Case 2: There is an unit being selected
            else if (selectedUnit != null)
            {
                //Checking clickPos
                ClearHighlightMap();
                if (gridManager.CheckPosition(clickPosition.x, clickPosition.y) == false)
                {
                    return;
                }
                //Case 2.1: There is new unit at clickPos, select it
                if (gridManager.GetUnit(clickPosition.x, clickPosition.y) != null)
                {
                    SelectUnitAtClickPos();
                    //Case 2.1.1: If new unit moved
                    if (selectedUnit.isMoved)
                    {
                        Debug.Log(selectedUnit.name + " moved");
                    }
                    //Case 2.1.2: If not moved, draw move range
                    else
                    {
                        DrawMoveRange();
                    }
                }
                //Case 2.2: No unit at clickPos, move current selected unit
                else
                {
                    MoveOnClick();
                    AfterMoving();
                    Deselect();
                }
            }
        }
    }

    private void SelectUnitAtClickPos()
    {
        ClearHighlightMap();
        if (gridManager.CheckPosition(clickPosition.x, clickPosition.y) == false)
        {
            return;
        }
        selectedUnit = gridManager.GetUnit(clickPosition.x, clickPosition.y);
    }

    private void DrawMoveRange()
    {
        List<PathNode> toHighlight = new List<PathNode>();
        pathFinding.Clear();
        pathFinding.CalculateWalkableTerrain(clickPosition.x, clickPosition.y, selectedUnit.moveDistance, ref toHighlight);

        for (int i = 0; i < toHighlight.Count; i++)
        {
            //highlightTilemap.SetTile(new Vector3Int(toHighlight[i].xPos, toHighlight[i].yPos, 0), highlightTile);
            Instantiate(highlightPrefab, new Vector3(toHighlight[i].xPos + 0.5f, toHighlight[i].yPos + 0.5f, 0), Quaternion.identity);
        }
    }

    private void MoveOnClick()
    {
        List<PathNode> path = pathFinding.TrackBackPath(selectedUnit, clickPosition.x, clickPosition.y);

        if (path != null)
        {
            path.Reverse();
            if (path.Count > 0)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    //highlightTilemap.SetTile(new Vector3Int(path[i].xPos, path[i].yPos, 0), highlightTile);
                    Instantiate(highlightPrefab, new Vector3(path[i].xPos + 0.5f, path[i].yPos + 0.5f, 0), Quaternion.identity);
                    selectedUnit.GetComponent<MapElement>().MoveUnit(path[i].xPos, path[i].yPos);
                }
            }
            selectedUnit.isMoved = true;
            ActionWait(); 
        }
        ClearHighlightMap();
    }

    private List<Vector2Int> GetAttackList()
    {
        Vector2Int unitPos = selectedUnit.GetUnitPos();
        int attackRange = selectedUnit.attackRange;
        List<Vector2Int> attackList = new List<Vector2Int>();
        if (attackRange == 1)
        {
            for (int x = attackRange * -1; x <= attackRange; x ++)
            {
                for (int y = attackRange * -1; y <= attackRange; y++)
                {
                    if ((Mathf.Abs(x) + Mathf.Abs(y)) > 0 && (Mathf.Abs(x) + Mathf.Abs(y)) <= attackRange)
                    {
                        if (gridManager.CheckPosition(unitPos.x + x, unitPos.y + y) == true)
                        {
                            attackList.Add(new Vector2Int(unitPos.x + x, unitPos.y + y));
                        }
                    }
                }
            }
        }
        else if (attackRange >= 1)
        {
            for (int x = attackRange * -1; x <= attackRange; x++)
            {
                for (int y = attackRange * -1; y <= attackRange; y++)
                {
                    if ((Mathf.Abs(x) + Mathf.Abs(y)) > 1 && (Mathf.Abs(x) + Mathf.Abs(y)) <= attackRange)
                    {
                        if (gridManager.CheckPosition(unitPos.x + x, unitPos.y + y) == true)
                        {
                            attackList.Add(new Vector2Int(unitPos.x + x, unitPos.y + y));
                        }
                    }
                }
            }
        }
        return attackList;
    }

    private List<Unit> CheckEnemy(List<Vector2Int> attackList)
    {
        List<Unit> attackableEnemy = new List<Unit>();
        foreach (Vector2Int pos in attackList)
        {
            Unit otherUnit = gridManager.GetUnit(pos.x, pos.y);
            if (otherUnit != null && otherUnit.unitColor != selectedUnit.unitColor)
            {
                attackableEnemy.Add(otherUnit);
            }
        }
        return attackableEnemy;
    }

    private void AfterMoving()
    {
        buttonWait.SetActive(true);
        buttonCancel.SetActive(true);
        List<Unit> attackableEnemy = CheckEnemy(GetAttackList());
        if (attackableEnemy.Count > 0)
        {
            buttonFire.SetActive(true);
        }
        foreach (Unit enemy in attackableEnemy)
        {
            Vector2Int enemyPos = enemy.GetUnitPos();
            //highlightTilemap.SetTile(new Vector3Int(enemyPos.x, enemyPos.y, 0), attackTile);
            Instantiate(attackPrefab, new Vector3(enemyPos.x + 0.5f, enemyPos.y + 0.5f, 0), Quaternion.identity);
        }
    }

    private void ClearHighlightMap()
    {
        GameObject[] highlightPrefabs = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject prefab in highlightPrefabs)
        {
            GameObject.Destroy(prefab);
        }
    }

    private void Deselect()
    {
        selectedUnit = null;
        pathFinding.Clear();
    }
}

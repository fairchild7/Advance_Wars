using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitControl : SimpleSingleton<UnitControl>
{
    public Tilemap targetTilemap;
    public Tilemap highlightTilemap;
    public GameObject highlightPrefab;
    public GameObject attackPrefab;
    public GridManager gridManager;

    public PathFinding pathFinding;

    public Unit selectedUnit;
    public Unit currentUnit;
    public Vector3 currentUnitPos;

    public Vector3Int clickPosition;
    public Tile tile;
    
    public void Awake()
    {
        pathFinding = targetTilemap.GetComponent<PathFinding>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(GameController.Instance.currentState);
        }
    }

    public void ActionWait()
    {
        ClearHighlightMap();
        currentUnit.isMoved = true;
        currentUnit.GetComponent<SpriteRenderer>().color = Color.gray;
        Deselect();
        UIGamePlay.Instance.HideAllButtons();
        GameController.Instance.ChangeState(new FreeState());
    }

    public void ActionCapture()
    {
        Building building = CheckBuilding(currentUnit.transform.position);
        Capture(currentUnit, building);
        ActionWait();
    }

    public void ActionFire()
    {
        UIGamePlay.Instance.buttonFire.SetActive(false);
        GameController.Instance.ChangeState(new FireState());

        List<Unit> attackableEnemy = GetEnemyInRange(GetAttackList(currentUnit, currentUnit.GetUnitPos()), currentUnit);

        foreach (Unit enemy in attackableEnemy)
        {
            Vector2Int enemyPos = enemy.GetUnitPos();
            Instantiate(attackPrefab, new Vector3(enemyPos.x + 0.5f, enemyPos.y + 0.5f, 0), Quaternion.identity);
        }
    }

    public void ActionEnd()
    {
        ClearHighlightMap();

        GridManager.Instance.RefreshUnit();
        UIGamePlay.Instance.buttonEnd.SetActive(false);
        GameController.Instance.StartEnemyTurn();
        GameController.Instance.ChangeState(new ViewState());
    }

    public void ActionCancel()
    {
        currentUnit.transform.position = currentUnitPos;
        currentUnit.GetComponent<MapElement>().PlaceObjectOnNewGrid(currentUnitPos);
        currentUnit.isMoved = false;
        selectedUnit = null;

        ClearHighlightMap();
        GameController.Instance.ChangeState(new FreeState());
        CheckBuilding(currentUnitPos);
    }

    public Building CheckBuilding(Vector2 position)
    {
        if (selectedUnit != null && (selectedUnit.unitType == UnitData.UnitType.Infantry || selectedUnit.unitType == UnitData.UnitType.Artillery))
        {
            List<Building> availableBuilding = new List<Building>();
            foreach (Building b in GameController.Instance.enemyBuilding)
            {
                availableBuilding.Add(b);
            }
            foreach (Building b in GameController.Instance.neutralBuilding)
            {
                availableBuilding.Add(b);
            }

            foreach (Building b in availableBuilding)
            {
                if (Vector2.Distance(position, b.transform.position) < 0.1f)
                {
                    UIGamePlay.Instance.buttonCapture.SetActive(true);
                    return b;
                }
            }
            UIGamePlay.Instance.buttonCapture.SetActive(false);
            selectedUnit.GetComponent<CapturableUnit>().StopCapturing();
            return null;
        }
        UIGamePlay.Instance.buttonCapture.SetActive(false);
        return null;
    }

    public bool CheckClickPos(int x, int y)
    {
        return gridManager.CheckPosition(x, y);
    }

    public void SelectUnitAtClickPos(int x, int y)
    {
        selectedUnit = gridManager.GetUnit(x, y);
    }

    public void DrawMoveRange(int x, int y)
    {
        List<PathNode> toHighlight = new List<PathNode>();
        pathFinding.Clear();
        pathFinding.CalculateWalkableTerrain(x, y, selectedUnit, ref toHighlight);

        for (int i = 0; i < toHighlight.Count; i++)
        {
            //highlightTilemap.SetTile(new Vector3Int(toHighlight[i].xPos, toHighlight[i].yPos, 0), highlightTile);
            Instantiate(highlightPrefab, new Vector3(toHighlight[i].xPos + 0.5f, toHighlight[i].yPos + 0.5f, 0), Quaternion.identity);
        }
    }

    public void MoveOnClick(Unit unit, int x, int y)
    {
        List<PathNode> path = pathFinding.TrackBackPath(unit, x, y);
        if (path != null)
        {
            path.Reverse();
            if (path.Count > 0)
            {
                StartCoroutine(unit.GetComponent<MapElement>().MoveOnList(path));

                unit.isMoved = true;
                if (CheckAllyUnit(unit))
                {
                    GameController.Instance.RefreshPosition(GameController.Instance.allyUnit);
                }
                else
                {
                    GameController.Instance.RefreshPosition(GameController.Instance.enemyUnit);
                }
            }
            else if (path.Count == 0)
            {
                ClearHighlightMap();
                Deselect();
            }
        }
        ClearHighlightMap();
    }

    public void MoveOnClick(int x, int y)
    {
        MoveOnClick(selectedUnit, x, y);
    }

    public List<Vector2Int> GetAttackList(Unit unit, Vector2Int unitPos)
    {
        if (unit != null)
        {
            int attackRange = unit.attackRange;
            List<Vector2Int> attackList = new List<Vector2Int>();
            if (attackRange == 1)
            {
                for (int x = attackRange * -1; x <= attackRange; x++)
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
        return null;
    }

    public List<Unit> GetEnemyInRange(List<Vector2Int> attackList, Unit unit)
    {
        List<Unit> attackableEnemy = new List<Unit>();
        foreach (Vector2Int pos in attackList)
        {
            Unit otherUnit = gridManager.GetUnit(pos.x, pos.y);
            if (otherUnit != null && otherUnit.GetUnitColor() != unit.GetUnitColor())
            {
                attackableEnemy.Add(otherUnit);
            }
        }
        return attackableEnemy;
    }

    public List<PathNode> GetMoveRange(Unit unit)
    {
        List<PathNode> moveRange = new List<PathNode>();
        pathFinding.Clear();
        pathFinding.CalculateWalkableTerrain((int)unit.transform.position.x, (int)unit.transform.position.y, unit, ref moveRange);
        return moveRange;
    }

    public List<PathNode> GetAttackRange(Unit unit)
    {
        List<PathNode> moveRange = new List<PathNode>();
        pathFinding.Clear();
        pathFinding.CalculateWalkableTerrain((int)unit.transform.position.x, (int)unit.transform.position.y, unit, ref moveRange);

        PathNode[,] pathNodes = pathFinding.GetPathNodes();

        List<PathNode> additionRange = new List<PathNode>();
        for (int i = 0; i < moveRange.Count; i++)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    if (GridManager.Instance.GetGridMap().CheckPosition(moveRange[i].xPos + x, moveRange[i].yPos + y) == false)
                    {
                        continue;
                    }
                    if (x != 0 && y != 0)
                    {
                        continue;
                    }
                    if (moveRange.Contains(pathNodes[moveRange[i].xPos + x, moveRange[i].yPos + y]))
                    {
                        continue;
                    }
                    additionRange.Add(pathNodes[moveRange[i].xPos + x, moveRange[i].yPos + y]);
                }
            }
        }

        for (int i = 0; i < additionRange.Count; i++)
        {
            moveRange.Add(additionRange[i]);
        }

        return moveRange;
    }

    public void CheckEnemy(Unit unit, Vector2Int unitPos)
    {
        List<Unit> attackableEnemy = GetEnemyInRange(GetAttackList(unit, unitPos), unit);
        if (attackableEnemy.Count > 0)
        {
            UIGamePlay.Instance.buttonFire.SetActive(true);
        }
    }

    public void ClearHighlightMap()
    {
        GameObject[] highlightPrefabs = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject prefab in highlightPrefabs)
        {
            GameObject.Destroy(prefab);
        }
    }

    public void Deselect()
    {
        selectedUnit = null;
        pathFinding.Clear();
    }

    public bool CheckAllyUnit(Unit unit)
    {
        return unit.GetUnitColor() == UnitData.UnitColor.Red;
    }

    public void Attack(Unit attackUnit, Unit targetUnit)
    {
        targetUnit.GetDamage(GameController.Instance.GetDamage(attackUnit, targetUnit));
        Debug.Log("Damage: " + GameController.Instance.GetDamage(attackUnit, targetUnit));
        if (attackUnit.attackRange == 1 && targetUnit.attackRange == 1)
        {
            float damageBack = GameController.Instance.GetDamage(targetUnit, attackUnit);
            if (damageBack < 0f)
            {
                return;
            }
            Debug.Log("Damage back: " + damageBack);
            attackUnit.GetDamage(damageBack);  
        }
    }

    public void Capture(Unit unit, Building building)
    {
        unit.GetComponent<CapturableUnit>().Capturing();
        building.GetDamage(Mathf.RoundToInt(unit.GetHp() / 10f));
        if (building.GetHp() <= 0)
        {
            building.ChangeColor(unit.GetUnitColor());
            unit.GetComponent<CapturableUnit>().StopCapturing();
        }
    }

    public bool IsHealing(Unit unit)
    {
        switch (unit.GetUnitColor())
        {
            case UnitData.UnitColor.Red:
                {
                    foreach (Building b in GameController.Instance.allyBuilding)
                    {
                        if (b != null)
                        {
                            if (Vector2.Distance(unit.transform.position, b.transform.position) < 0.1f)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
            case UnitData.UnitColor.Blue:
                {
                    foreach (Building b in GameController.Instance.enemyBuilding)
                    {
                        if (b != null)
                        {
                            if (Vector2.Distance(unit.transform.position, b.transform.position) < 0.1f)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
        }
        return false;
    }
}

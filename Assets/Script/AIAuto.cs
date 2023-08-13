using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAuto : SimpleSingleton<AIAuto>
{
    public IEnumerator ExecuteTurn()
    {
        foreach (Unit u in GameController.Instance.enemyUnit)
        {
            OnEnemyTurn(u);
            yield return new WaitForSeconds(0.5f);
        }
        GameController.Instance.StartPlayerTurn();
    }

    public void OnEnemyTurn(Unit unit)
    {
        //Debug.Log("_" + unit.name);
        if (HuntingState(unit))
        {
            MoveAndAttack(unit);
        }
        else
        {
            if (unit.GetHp() >= 70f)
            {
                if (unit.unitType == UnitData.UnitType.Infantry || unit.unitType == UnitData.UnitType.Artillery)
                {
                    if (CapturingState(unit) != null)
                    {
                        UnitControl.Instance.Capture(unit, CapturingState(unit));
                    }
                    else
                    {
                        MoveAndCapture(unit);
                    }
                }
                else
                {
                    MoveAndAttack(unit);
                }
            }
            else
            {
                if (unit.unitType == UnitData.UnitType.Infantry || unit.unitType == UnitData.UnitType.Artillery)
                {
                    if (CapturingState(unit) != null && CapturingState(unit).GetHp() <= unit.GetHp())
                    {
                        UnitControl.Instance.Capture(unit, CapturingState(unit));
                    }
                    else
                    {
                        Run(unit);
                    }
                }
                else
                {
                    Run(unit);
                }
            }
        }
    }

    private bool HuntingState(Unit unit)
    {
        List<Unit> enemyUnitInRange = CheckAttackableEnemy(unit);

        foreach (Unit u in enemyUnitInRange)
        {
            if (u.GetHp() < 40f && unit.GetHp() > u.GetHp())
            {
                return true;
            }
        }
        return false;
    }

    private Building CapturingState(Unit unit)
    {
        List<Building> availableBuilding = new List<Building>();
        foreach (Building b in GameController.Instance.allyBuilding)
        {
            availableBuilding.Add(b);
        }
        foreach (Building b in GameController.Instance.neutralBuilding)
        {
            availableBuilding.Add(b);
        }

        foreach (Building b in availableBuilding)
        {
            if (b != null)
            {
                if (Vector2.Distance(unit.transform.position, b.transform.position) < 0.1f)
                {
                    return b;
                }
            }
        }

        return null;
    }
    
    private void Run(Unit unit)
    {
        foreach (Building b in GameController.Instance.enemyBuilding)
        {
            if (Vector2.Distance(unit.transform.position, b.transform.position) < 0.1f)
            {
                return;
            }
        }

        List<PathNode> path = FindNearestAllyBuilding(unit);
        if (path.Count > 0)
        {
            path.Reverse();
            if (unit.moveDistance > path.Count)
            {
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    if (GridManager.Instance.GetGridMap().GetUnit(path[i].xPos, path[i].yPos) == null)
                    {
                        UnitControl.Instance.MoveOnClick(unit, path[i].xPos, path[i].yPos);
                        GameController.Instance.RefreshPosition(GameController.Instance.enemyUnit);
                        return;
                    }
                }
                return;
            }

            for (int i = unit.moveDistance - 1; i >= 0; i--)
            {
                if (GridManager.Instance.GetGridMap().GetUnit(path[i].xPos, path[i].yPos) == null)
                {
                    UnitControl.Instance.MoveOnClick(unit, path[i].xPos, path[i].yPos);
                    GameController.Instance.RefreshPosition(GameController.Instance.enemyUnit);
                    return;
                }
            }
        }
    }

    private void MoveAndAttack(Unit unit)
    {
        List<Unit> targetUnits = CheckAttackableEnemy(unit);
        while (targetUnits.Count > 0)
        {
            Unit targetUnit = GetMostEffectiveUnitToAttack(unit, targetUnits);
            PathNode attackPos = PositionToAttack(unit, targetUnit);
            if (attackPos == null)
            {
                targetUnits.Remove(targetUnit);
            }
            else
            {
                UnitControl.Instance.MoveOnClick(unit, attackPos.xPos, attackPos.yPos);
                //Debug.Log(unit.GetHp());
                //Debug.Log(targetUnit.GetHp());
                UnitControl.Instance.Attack(unit, targetUnit);
                return;
            }
        }

        if (targetUnits.Count == 0)
        {
            MoveToNearestEnemy(unit);
        }
    }

    private void MoveAndCapture(Unit unit)
    {
        List<PathNode> path = FindNearestEnemyBuilding(unit).Item2;
        Building targetBuilding = FindNearestEnemyBuilding(unit).Item1;
        if (path.Count > 0)
        {
            path.Reverse();
            if (unit.moveDistance > path.Count)
            {
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    if (GridManager.Instance.GetGridMap().GetUnit(path[i].xPos, path[i].yPos) == null)
                    {
                        UnitControl.Instance.MoveOnClick(unit, path[i].xPos, path[i].yPos);
                        //Debug.Log("x:" + path[i].xPos + " y:" + path[i].yPos);
                        //Debug.Log(Vector2.Distance(new Vector2(path[i].xPos, path[i].yPos), targetBuilding.transform.position));
                        if (Vector2.Distance(new Vector2(path[i].xPos + 0.5f, path[i].yPos + 0.5f), targetBuilding.transform.position) < 0.1f)
                        {
                            UnitControl.Instance.Capture(unit, targetBuilding);
                        }
                        GameController.Instance.RefreshPosition(GameController.Instance.enemyUnit);
                        return;
                    }
                }
                return;
            }

            for (int i = unit.moveDistance - 1; i >= 0; i--)
            {
                if (GridManager.Instance.GetGridMap().GetUnit(path[i].xPos, path[i].yPos) == null)
                {
                    UnitControl.Instance.MoveOnClick(unit, path[i].xPos, path[i].yPos);
                    //Debug.Log("x:" + path[i].xPos + " y:" + path[i].yPos);
                    //Debug.Log(Vector2.Distance(new Vector2(path[i].xPos, path[i].yPos), targetBuilding.transform.position));
                    if (Vector2.Distance(new Vector2(path[i].xPos + 0.5f, path[i].yPos + 0.5f), targetBuilding.transform.position) < 0.1f)
                    {  
                        UnitControl.Instance.Capture(unit, targetBuilding);
                    }
                    GameController.Instance.RefreshPosition(GameController.Instance.enemyUnit);
                    return;
                }
            }
        }
    }

    private List<PathNode> FindNearestAllyBuilding(Unit unit)
    {
        int minPath = 999;

        List<PathNode> moveRange = new List<PathNode>();
        UnitControl.Instance.pathFinding.Clear();
        UnitControl.Instance.pathFinding.CalculateWalkableTerrain((int)unit.transform.position.x, (int)unit.transform.position.y, 99, unit, ref moveRange);

        List<PathNode> chosenPath = new List<PathNode>();
        foreach (Building b in GameController.Instance.enemyBuilding)
        {
            Vector2Int targetPos = new Vector2Int((int)b.transform.position.x, (int)b.transform.position.y);

            if (GridManager.Instance.GetGridMap().GetUnit(targetPos.x, targetPos.y) != null)
            {
                continue;
            }

            List<PathNode> path = UnitControl.Instance.pathFinding.TrackBackPath(unit, targetPos.x, targetPos.y);
            if (path.Count > 0)
            {
                if (path.Count < minPath)
                {
                    chosenPath = path;
                    minPath = path.Count;
                }
            }
        }
        return chosenPath;
    }

    private (Building, List<PathNode>) FindNearestEnemyBuilding(Unit unit)
    {
        int minPath = 999;
        Building nearestBuilding = null;

        List<Building> availableBuilding = new List<Building>();
        foreach (Building b in GameController.Instance.allyBuilding)
        {
            availableBuilding.Add(b);
        }
        foreach (Building b in GameController.Instance.neutralBuilding)
        {
            availableBuilding.Add(b);
        }

        List<PathNode> moveRange = new List<PathNode>();
        UnitControl.Instance.pathFinding.Clear();
        UnitControl.Instance.pathFinding.CalculateWalkableTerrain((int)unit.transform.position.x, (int)unit.transform.position.y, 99, unit, ref moveRange);

        List<PathNode> chosenPath = new List<PathNode>();
        foreach (Building b in availableBuilding)
        {

            if (b != null)
            {
                Vector2Int targetPos = new Vector2Int((int)b.transform.position.x, (int)b.transform.position.y);

                if (GridManager.Instance.GetGridMap().GetUnit(targetPos.x, targetPos.y) != null)
                {
                    continue;
                }

                List<PathNode> path = UnitControl.Instance.pathFinding.TrackBackPath(unit, targetPos.x, targetPos.y);
                if (path.Count > 0)
                {
                    if (path.Count < minPath)
                    {
                        nearestBuilding = b;
                        chosenPath = path;
                        minPath = path.Count;
                    }
                }
            }
        }

        return (nearestBuilding, chosenPath);
    }

    private void MoveToNearestEnemy(Unit unit)
    {
        Unit target = FindNearestEnemy(unit).Item2;
        if (target != null)
        {
            List<PathNode> path = FindNearestEnemy(unit).Item1;
            path.Reverse();

            if (unit.moveDistance > path.Count)
            {
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    if (GridManager.Instance.GetGridMap().GetUnit(path[i].xPos, path[i].yPos) == null)
                    {
                        UnitControl.Instance.MoveOnClick(unit, path[i].xPos, path[i].yPos);
                        GameController.Instance.RefreshPosition(GameController.Instance.enemyUnit);
                        return;
                    }
                }
                return;
            }

            for (int i = unit.moveDistance - 1; i >= 0; i--)
            {
                //Debug.Log("i = " + i + "; xPos = " + path[i].xPos + "; yPos = " + path[i].yPos);
                if (GridManager.Instance.GetGridMap().GetUnit(path[i].xPos, path[i].yPos) == null)
                {
                    UnitControl.Instance.MoveOnClick(unit, path[i].xPos, path[i].yPos);
                    GameController.Instance.RefreshPosition(GameController.Instance.enemyUnit);
                    return;
                }
            }
        }
    }

    private (List<PathNode>, Unit) FindNearestEnemy(Unit unit)
    {
        int minPath = 999;
        Unit nearestEnemy = null;

        List<PathNode> chosenPath = new List<PathNode>();
        foreach (Unit u in GameController.Instance.allyUnit)
        {
            List<PathNode> moveRange = new List<PathNode>();
            UnitControl.Instance.pathFinding.Clear();
            UnitControl.Instance.pathFinding.CalculateWalkableTerrain((int)unit.transform.position.x, (int)unit.transform.position.y, 99, unit, ref moveRange);

            Vector2Int enemyPos = new Vector2Int((int)u.transform.position.x, (int)u.transform.position.y);
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    if (x != 0 && y != 0)
                    {
                        continue;
                    }
                    if (GridManager.Instance.GetGridMap().CheckPosition(enemyPos.x + x, enemyPos.y + y) == false)
                    {
                        continue;
                    }
                    List<PathNode> path = UnitControl.Instance.pathFinding.TrackBackPath(unit, enemyPos.x + x, enemyPos.y + y);
                    if (path.Count > 0)
                    {
                        if (path.Count < minPath)
                        {
                            nearestEnemy = u;
                            chosenPath = path;
                            minPath = path.Count;
                        }
                    }
                }
            }
        }
        return (chosenPath, nearestEnemy);
    }

    private List<Unit> CheckAttackableEnemy(Unit unit)
    {
        List<PathNode> attackRange = UnitControl.Instance.GetAttackRange(unit);
        List<Unit> attackableUnit = new List<Unit>();

        foreach (PathNode node in attackRange)
        {
            Unit enemyUnit = GridManager.Instance.GetUnit(node.xPos, node.yPos);
            if (enemyUnit != null && enemyUnit.GetUnitColor() == UnitData.UnitColor.Red)
            {
                if (!attackableUnit.Contains(enemyUnit))
                {
                    attackableUnit.Add(enemyUnit);
                }
            }
        }
        return attackableUnit;
    }

    private Unit GetMostEffectiveUnitToAttack(Unit attackUnit, List<Unit> targetUnit)
    {
        Unit effectiveUnit = null;
        float maxDamage = 0f;
        foreach (Unit u in targetUnit)
        {
            if (GameController.Instance.GetDamage(attackUnit, u) > maxDamage)
            {
                effectiveUnit = u;
                maxDamage = GameController.Instance.GetDamage(attackUnit, u);
            }
        }
        return effectiveUnit;
    }

    private PathNode PositionToAttack(Unit attackUnit, Unit targetUnit)
    {
        List<PathNode> legalPosition = new List<PathNode>();
        List<PathNode> moveRange = UnitControl.Instance.GetMoveRange(attackUnit);
        foreach (PathNode node in moveRange)
        {
            if (Mathf.Abs(node.xPos - (int)targetUnit.transform.position.x) + Mathf.Abs(node.yPos - (int)targetUnit.transform.position.y) == 1)
            {
                Unit allyUnit = GridManager.Instance.GetUnit(node.xPos, node.yPos);
                if (allyUnit != null && allyUnit != attackUnit)
                {
                    continue;
                }
                else
                {
                    legalPosition.Add(node);
                }
            }
        }

        int maxDefense = -1;
        PathNode targetNode = null;
        foreach (PathNode node in legalPosition)
        {
            TerrainType terrainType = GridManager.Instance.GetTerrainType(node.xPos, node.yPos);
            int terrainDefense = EnvironmentData.Instance.defenseValue[(int)terrainType];
            if (terrainDefense > maxDefense)
            {
                targetNode = node;
                maxDefense = terrainDefense;
            }
        }
        return targetNode;
    }
}

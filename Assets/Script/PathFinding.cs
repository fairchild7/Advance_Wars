using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public TerrainType terrainType;
    public int xPos;
    public int yPos;
    public float gValue;
    public float hValue;
    public PathNode parentNode;

    public float fValue
    {
        get
        {
            return gValue + hValue;
        }
    }

    public PathNode(int xPos, int yPos, TerrainType terrainType)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        this.terrainType = terrainType;
    }

    public void Clear()
    {
        gValue = 0;
        hValue = 0;
        parentNode = null;
    }
}

[RequireComponent(typeof(GridMap))]
public class PathFinding : MonoBehaviour
{
    GridMap gridMap;
    GridManager gridManager;
    PathNode[,] pathNodes;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (gridMap == null)
        {
            gridMap = GetComponent<GridMap>();
        }
        if (gridManager == null)
        {
            gridManager = GetComponent<GridManager>();
        }

        pathNodes = new PathNode[gridMap.width, gridMap.height];
        
        for (int x = 0; x < gridMap.width; x++)
        {
            for (int y = 0; y < gridMap.height; y++)
            {
                pathNodes[x, y] = new PathNode(x, y, gridManager.GetTerrainType(x, y));
            }
        }
    }

    public void CalculateWalkableTerrain(int startX, int startY, int range, ref List<PathNode> toHighlight)
    {
        range *= 10;

        PathNode startNode = pathNodes[startX, startY];

        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        openList.Add(startNode);

        while(openList.Count > 0)
        {
            PathNode currentNode = openList[0];

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<PathNode> neighbourNodes = new List<PathNode>();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0) //Not add current node to neighbour list
                    {
                        continue;
                    }
                    if (gridMap.CheckPosition(currentNode.xPos + x, currentNode.yPos + y) == false)
                    {
                        continue;
                    }
                    if (x != 0 && y != 0) //Not diagonal walkable (if want diagonal walkable, remove this condition)
                    {
                        continue;
                    }
                    neighbourNodes.Add(pathNodes[currentNode.xPos + x, currentNode.yPos + y]);
                }
            }

            for (int i = 0; i < neighbourNodes.Count; i++)
            {
                if (closedList.Contains(neighbourNodes[i]))
                {
                    continue;
                }
                /*
                if (gridMap.CheckWalkable(neighbourNodes[i].xPos, neighbourNodes[i].yPos) == false)
                {
                    continue;
                }
                */
                //This is where terrain affects
                float moveCost = CalculateDistance(currentNode, neighbourNodes[i]) * TerrainMultiplicator(neighbourNodes[i]);
                float totalMoveCost = currentNode.gValue + moveCost;

                if (totalMoveCost > range)
                {
                    continue;
                }

                if (closedList.Contains(neighbourNodes[i]) == false || moveCost < neighbourNodes[i].gValue)
                {
                    neighbourNodes[i].gValue = totalMoveCost;
                    neighbourNodes[i].parentNode = currentNode;
                    if (openList.Contains(neighbourNodes[i]) == false)
                    {
                        openList.Add(neighbourNodes[i]);
                    }
                }
            }
        }

        if (toHighlight != null)
        {
            toHighlight.AddRange(closedList);
        }
    }

    private float TerrainMultiplicator(PathNode pathNode)
    {
        switch (pathNode.terrainType)
        {
            case TerrainType.Grass:
                return 1f;
            case TerrainType.Road:
                return 1f;
            case TerrainType.Tree:
                return 2f;
            case TerrainType.River:
                return 3f;
            case TerrainType.Mountain:
                return 3f;
        }
        return 1f;
    }

    internal void Clear()
    {
        for (int x = 0; x < gridMap.width; x++)
        {
            for (int y = 0; y < gridMap.height; y++)
            {
                pathNodes[x, y].Clear();
            }
        }
    }

    public List<PathNode> TrackBackPath(Unit selectedUnit, int x, int y)
    {
        List<PathNode> path = new List<PathNode>();
        if (gridMap.CheckPosition(x,y) == false)
        {
            return null;
        }
        PathNode currentNode = pathNodes[x, y];

        while (currentNode.parentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        return path;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = pathNodes[startX, startY];
        PathNode endNode = pathNodes[endX, endY];

        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        openList.Add(startNode);

        while(openList.Count > 0)
        {
            PathNode currentNode = openList[0];
            for (int i = 0; i < openList.Count; i++)
            {
                if (currentNode.fValue > openList[i].fValue)
                {
                    currentNode = openList[i];
                }
                if (currentNode.fValue == openList[i].fValue && currentNode.hValue > openList[i].hValue)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == endNode)
            {
                //Finished
                return RetracePath(startNode, endNode);
            }

            List<PathNode> neighbourNodes = new List<PathNode>();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0) //Not add current node to neighbour list
                    {
                        continue;
                    }
                    if (gridMap.CheckPosition(currentNode.xPos + x, currentNode.yPos + y) == false)
                    {
                        continue;
                    }
                    if (x != 0 && y != 0) //Not diagonal walkable (if want diagonal walkable, remove this condition)
                    {
                        continue;
                    }
                    neighbourNodes.Add(pathNodes[currentNode.xPos + x, currentNode.yPos + y]);
                }
            }

            for (int i = 0; i < neighbourNodes.Count; i++)
            {
                if (closedList.Contains(neighbourNodes[i]))
                {
                    continue;
                }
                if (gridMap.CheckWalkable(neighbourNodes[i].xPos, neighbourNodes[i].yPos) == false)
                {
                    continue;
                }

                float movementCost = currentNode.gValue + CalculateDistance(currentNode, neighbourNodes[i]);

                if (openList.Contains(neighbourNodes[i]) == false || movementCost < neighbourNodes[i].gValue)
                {
                    neighbourNodes[i].gValue = movementCost;
                    neighbourNodes[i].hValue = CalculateDistance(neighbourNodes[i], endNode);
                    neighbourNodes[i].parentNode = currentNode;

                    if (openList.Contains(neighbourNodes[i]) == false)
                    {
                        openList.Add(neighbourNodes[i]);
                    }
                }
            }
        }
        return null;
    }

    private List<PathNode> RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();

        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        path.Reverse(); 

        return path;
    }

    private float CalculateDistance(PathNode current, PathNode target)
    {
        int distX = Mathf.Abs(current.xPos - target.xPos);
        int distY = Mathf.Abs(current.yPos - target.yPos);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
    }
}

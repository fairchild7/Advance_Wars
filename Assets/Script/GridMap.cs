using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Node
{
    public int tileId;
    public Unit unit;
}

public class GridMap : MonoBehaviour
{
    [HideInInspector]
    public int height;

    [HideInInspector]
    public int width;

    Node[,] grid;

    public void Init(int width, int height)
    {
        grid = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new Node();
            }
        }
        this.width = width;
        this.height = height;
    }

    public void SetTile(int x, int y, int to)
    {
        if (CheckPosition(x, y) == false)
        {
            Debug.LogWarning("Trying to Set an cell outside the Grid boundaries "
                + x.ToString() + ":" + y.ToString());
            return;
        }
        grid[x, y].tileId = to;
    }

    public int GetTile(int x, int y)
    {
        if (CheckPosition(x, y) == false)
        {
            Debug.LogWarning("Trying to Get an cell outside the Grid boundaries " 
                + x.ToString() + ":" + y.ToString());
            return -1;
        }
        return grid[x, y].tileId;
    }

    public bool CheckPosition(int x, int y)
    {
        if (x < 0 || x >= width)
        {
            return false;
        }
        if (y < 0 || y >= height)
        {
            return false;
        }
        return true;
    }

    internal bool CheckWalkable(int xPos, int yPos)
    {
        return grid[xPos, yPos].tileId >= 0;
    }

    internal void SetUnit(MapElement mapElement, int x_pos, int y_pos)
    {
        grid[x_pos, y_pos].unit = mapElement.GetComponent<Unit>();
    }

    public Unit GetUnit(int x, int y)
    {
        return grid[x, y].unit;
    }

    internal void ClearUnit(int x_pos, int y_pos)
    {
        grid[x_pos, y_pos].unit = null;
    }
}

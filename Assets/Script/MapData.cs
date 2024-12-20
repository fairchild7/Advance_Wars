using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class MapData : ScriptableObject
{
    public int width, height;

    public List<int> map;

    public void Load(GridMap gridMap)
    {
        gridMap.Init(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridMap.SetTile(x, y, Get(x, y));
            }
        }
    }

    private int Get(int x, int y)
    {
        int index = x * height + y;
        if (index >= map.Count)
        {
            Debug.LogError("Out of range on the map data!");
            return -1;
        }
        return map[index];
    }

    public void Save(GridMap gridMap)
    {
        height = gridMap.height;
        width = gridMap.width;

        map = new List<int>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map.Add(gridMap.GetTile(x, y));
            }
        }
    }

    internal void Save(int[,] map)
    {
        width = map.GetLength(0);
        height = map.GetLength(1);

        this.map = new List<int>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                this.map.Add(map[x, y]);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : SimpleSingleton<GridManager>
{
    Tilemap tilemap;
    GridMap grid;
    SaveLoadMap saveLoadMap;

    [SerializeField] TileSet tileSet;

    private void Awake()
    {

    }

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        grid = GetComponent<GridMap>();
        saveLoadMap = GetComponent<SaveLoadMap>();

        //grid.Init(15, 10);
        saveLoadMap.Save();
        tilemap.ClearAllTiles();
        saveLoadMap.Load(grid);
        UpdateTileMap();
    }

    internal void Clear()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }
        tilemap.ClearAllTiles();
        tilemap = null;
    }

    public void SetTile(int x, int y, int tileid)
    {
        if (tileid == -1)
        {
            return;
        }
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }
        tilemap.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[tileid]);
        tilemap = null;
    }

    public TerrainType GetTerrainType(int x, int y)
    {
        int tileId = grid.GetTile(x, y);
        return tileSet.terrainData.terrains[tileId];
    }

    internal bool CheckPosition(int x, int y)
    {
        return grid.CheckPosition(x, y);
    }

    void UpdateTileMap()
    {
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                UpdateTile(x, y);
            }
        }
    }

    public Unit GetUnit(int x, int y)
    {
        return grid.GetUnit(x, y);
    }

    private void UpdateTile(int x, int y)   
    {
        int tileId = grid.GetTile(x, y);
        if (tileId == -1)
        {
            return;
        }

        tilemap.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[tileId]);
    }

    public void Set(int x, int y, int to)
    {
        grid.SetTile(x, y, to);
        UpdateTile(x, y);
    }

    public int[,] ReadTileMap()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }
        int size_x = tilemap.size.x;
        int size_y = tilemap.size.y;
        int[,] tilemapdata = new int[size_x, size_y];

        for (int x = 0; x < size_x; x++)
        {
            for (int y = 0; y < size_y; y++)
            {
                TileBase tileBase = tilemap.GetTile(new Vector3Int(x, y, 0));
                //something wrong?
                int indexTile = tileSet.tiles.FindIndex(index => index == tileBase);
                tilemapdata[x, y] = indexTile;
            }
        }
        return tilemapdata;
    }

    public void RefreshUnit()
    {
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                Unit unit = GetUnit(x, y);
                if (unit != null)
                {
                    unit.isMoved = false;
                    unit.GetComponent<SpriteRenderer>().color = unit.originalColor;
                }
            }
        }
    }

    public GridMap GetGridMap()
    {
        return grid;
    }
}

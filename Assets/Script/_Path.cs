using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class _Path : MonoBehaviour
{
    [SerializeField]
    Sprite iconSelect, iconSelectFire;
    private Transform select;
    CellPath[,] layerPath;
    List<List<Vector2>> listListPaths;
    public List<Vector2> listUnitMove;

    private int mapWidth = Manager.mapWidth;
    private int mapHeight = Manager.mapHeight;
    private int firstCellX = Manager.firstCellX;
    private int firstCellY = Manager.firstCellY;
    public static bool[,] isArea = new bool[Manager.mapWidth, Manager.mapHeight];

    public void MyAwake()
    {
        layerPath = new CellPath[mapWidth, mapHeight];
        listListPaths = new List<List<Vector2>>();
        for (int r = 0; r < mapHeight; r++)
        {
            for (int c = 0; c < mapWidth; c++)
                layerPath[r, c] = new CellPath();
        }
    }

    public bool CheckArea(int row, int column)
    {
        return layerPath[row, column].isArea;
    }

    public void ClearArea()
    {
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }
        foreach (Transform child in children)
        {
            Destroy(child.gameObject);
        }
        for (int c = 0; c < mapHeight; c++)
        {
            for (int r = 0; r < mapWidth; r++)
            {
                isArea[r, c] = false;
            }
        }
    }

    public void FindArea(GameInformation.Unit unit, int move, float x, float y)
    {
        if (move < 0)
            return;
        else
        {
            isArea[(int)(x - firstCellX), (int)(y - firstCellY)] = true;
            if (x + 1 <= firstCellX + mapWidth)
                FindArea(unit, move - 1, x + 1, y);
            if (y + 1 <= firstCellY + mapHeight)
                FindArea(unit, move - 1, x, y + 1);
            if (x - 1 >= firstCellX)
                FindArea(unit, move - 1, x - 1, y);
            if (y - 1 >= firstCellY)
                FindArea(unit, move - 1, x, y - 1);
        }
    }

    public void DrawArea()
    {
        for (int c = 0; c < mapHeight; c++)
        {
            for (int r = 0; r < mapWidth; r++)
            {
                if (isArea[r, c])
                    Instantiate((Resources.Load("CellGreen") as GameObject), transform).transform.position = new Vector2(r + firstCellX, c + firstCellY + 0.5f);
            }
        }
    }

    public void ClearPath()
    {

    }

    public void DrawPath()
    {
        listListPaths.Clear();
        ClearPath();
        if (layerPath[Manager.cursorX, Manager.cursorY].isArea)
        {
            if (Manager.selectedX == Manager.cursorX && Manager.selectedY == Manager.cursorY)
            {
                listUnitMove.Add(new Vector2(Manager.cursorX, Manager.cursorY));
                Instantiate((Resources.Load("CellGreen") as GameObject), transform.GetChild(0)).transform.position = new Vector2(Manager.cursorX, Manager.cursorY);
                return;
            }
            else
            {
                //return;
            }
            List<Vector2> path = new List<Vector2>();
            FindPath(Manager.selectedX, Manager.selectedY, Manager.unit.move, path);
            int min = 100;
            for (int i = 0; i < listListPaths.Count; i++)
            {
                if (min > listListPaths[i].Count)
                {
                    min = listListPaths[i].Count;
                }
            }
            for (int i = 0; i < listListPaths.Count; i++)
            {
                if (listListPaths.Count == min)
                {
                    listUnitMove = new List<Vector2>();
                    for (int j = 0; j < min; j++)
                    {
                        listUnitMove.Add(listListPaths[i][j]);
                        layerPath[(int)listListPaths[i][j].x, (int)listListPaths[i][j].y].checkDraw = true;
                    }
                    break;
                }
            }
            for (int c = 0; c < mapHeight; c++)
            {
                for (int r = 0; r < mapWidth; r++)
                {
                    if (layerPath[r,c].checkDraw)
                    {
                        Instantiate((Resources.Load("CellGreen") as GameObject), transform.GetChild(0)).transform.position = new Vector2(r, c);
                    }
                }
            }
        }  
    }

    private void FindPath(int xStart, int yStart, int move, List<Vector2> path)
    {
        List<Vector2> pathList = new List<Vector2>();
        for (int i = 0; i < path.Count; i++)
        {
            pathList.Add(path[i]);
        }
        pathList.Add(new Vector2(xStart, yStart));
        if (xStart == Manager.cursorX && yStart == Manager.cursorY)
        {
            listListPaths.Add(pathList);
            return;
        }
        if (move == 0)
        {
            return;
        }
        else
        {
            if (yStart + 1 <= mapHeight - 1)
            {
                if (layerPath[xStart, yStart + 1].isArea)
                {
                    FindPath(xStart, yStart + 1, move - 1, path);
                }
            }
            if (yStart - 1 >= 0)
            {
                if (layerPath[xStart, yStart - 1].isArea)
                {
                    FindPath(xStart, yStart - 1, move - 1, path);
                }
            }
            if (xStart + 1 <= mapWidth - 1)
            {
                if (layerPath[xStart + 1, yStart].isArea)
                {
                    FindPath(xStart + 1, yStart, move - 1, path);
                }
            }
            if (xStart - 1 >= 0)
            {
                if (layerPath[xStart - 1, yStart].isArea)
                {
                    FindPath(xStart - 1, yStart, move - 1, path);
                }
            }
        }
    }
}

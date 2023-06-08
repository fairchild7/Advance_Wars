using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitControl : MonoBehaviour
{
    [SerializeField] Tilemap targetTilemap;
    [SerializeField] Tilemap highlightTilemap;
    [SerializeField] TileBase highlightTile;
    [SerializeField] GridManager gridManager;
 
    PathFinding pathFinding;
    Unit selectedUnit;

    private void Awake()
    {
        pathFinding = targetTilemap.GetComponent<PathFinding>();    
    }

    private void Update()
    {
        MouseInput();
    }

    private void MouseInput()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int clickPosition = targetTilemap.WorldToCell(worldPoint);
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right " + clickPosition.x + " " + clickPosition.y);
            highlightTilemap.ClearAllTiles();
            if (gridManager.CheckPosition(clickPosition.x, clickPosition.y) == false)
            {
                return;
            }
            selectedUnit = gridManager.GetUnit(clickPosition.x, clickPosition.y);
            if (selectedUnit == null) Debug.Log("Fking null");
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

    private void Deselect()
    {
        selectedUnit = null;
        pathFinding.Clear();
    }
}

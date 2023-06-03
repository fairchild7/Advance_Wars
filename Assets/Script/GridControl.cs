using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridControl : MonoBehaviour
{
    [SerializeField] Tilemap targetTilemap;
    [SerializeField] Tilemap highlightTilemap;
    [SerializeField] GridManager gridManager;
    PathFinding pathFinding;
    GameObject pointer;

    int currentX = 0;
    int currentY = 0;
    int targetPosX = 0;
    int targetPosY = 0;

    [SerializeField] TileBase highlightTile;

    private void Start()
    {
        pathFinding = gridManager.GetComponent<PathFinding>();
        pointer = Instantiate((Resources.Load("Pointer") as GameObject), transform);
        pointer.transform.position = new Vector3(0.5f, 0.5f, 0);
    }

    private void Update()
    {
        MouseInput();
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickPosition = targetTilemap.WorldToCell(worldPoint);

            //highlightTilemap.ClearAllTiles();
            targetPosX = clickPosition.x;
            targetPosY = clickPosition.y;

            if (gridManager.CheckPosition(targetPosX, targetPosY))
            {
                pointer.transform.position = new Vector3(clickPosition.x + 0.5f, clickPosition.y + 0.5f, -0.5f);
            }

            currentX = clickPosition.x;
            currentY = clickPosition.y;
        }
    }
}

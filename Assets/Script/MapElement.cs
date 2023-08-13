using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapElement : MonoBehaviour
{
    GridMap gridMap;
    Animator animator;
    int x_pos = 0;
    int y_pos = 0;

    public float unitMoveSpeed = 5f;

    private string currentAnimation;

    void Start()
    {
        SetGrid();
        PlaceObjectOnGrid();
        animator = GetComponent<Animator>();
        currentAnimation = "idle";

        Unit unit = GetComponent<Unit>();
        if (unit.GetUnitColor() == UnitData.UnitColor.Red)
        {
            GameController.Instance.allyUnit.Add(unit);
        }
        else if (unit.GetUnitColor() == UnitData.UnitColor.Blue)
        {
            GameController.Instance.enemyUnit.Add(unit);
        }
    }

    private void SetGrid()
    {
        gridMap = transform.parent.GetComponent<GridMap>();
    }

    public IEnumerator MoveOnList(List<PathNode> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            //yield return StartCoroutine(MoveUnit(path[i].xPos, path[i].yPos));
            MoveUnit(path[i].xPos, path[i].yPos);
        }
        ChangeAnimation("idle");
        if (GetComponent<Unit>().GetUnitColor() == UnitData.UnitColor.Red)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (GetComponent<Unit>().GetUnitColor() == UnitData.UnitColor.Blue)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        yield return null;
    }

    public void MoveUnit(int targetPosX, int targetPosY)
    {
        RemoveObjectFromGrid();
        MoveTo(targetPosX, targetPosY);
        Vector3 worldPosition = new Vector3(x_pos * 1f + 0.5f, y_pos * 1f + 0.5f, -0.5f);

        transform.position = worldPosition;
        /*
        while (Vector2.Distance(transform.position, worldPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, worldPosition, unitMoveSpeed * Time.deltaTime);
            MoveAnimation(transform.position, worldPosition);
            yield return null;
        }
        */
    }

    public void MoveAnimation(Vector2 startPos, Vector2 endPos)
    {
        float xDiff = endPos.x - startPos.x;
        float yDiff = endPos.y - startPos.y;
        if (MathF.Abs(xDiff) > MathF.Abs(yDiff))
        {
            if (xDiff > 0)
            {
                if (GetComponent<Unit>().GetUnitColor() == UnitData.UnitColor.Red)
                {
                    ChangeAnimation("hori");
                }
                else if (GetComponent<Unit>().GetUnitColor() == UnitData.UnitColor.Blue)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    ChangeAnimation("hori");
                }
            }
            else
            {
                if (GetComponent<Unit>().GetUnitColor() == UnitData.UnitColor.Red)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    ChangeAnimation("hori");
                }
                else if (GetComponent<Unit>().GetUnitColor() == UnitData.UnitColor.Blue)
                {
                    ChangeAnimation("hori");
                }
            }
        }
        else if (MathF.Abs(xDiff) < MathF.Abs(yDiff))
        {
            if (yDiff > 0)
            {
                ChangeAnimation("up");
            }
            else
            {
                ChangeAnimation("down");
            }
        }
    }

    public Vector2Int GetUnitPos()
    {
        return new Vector2Int(this.x_pos, this.y_pos);
    }

    private void MoveTo(int targetPosX, int targetPosY)
    {
        gridMap.SetUnit(this, targetPosX, targetPosY);
        x_pos = targetPosX;
        y_pos = targetPosY;
    }
    public void PlaceObjectOnNewGrid(Vector3 pos)
    {
        RemoveObjectFromGrid();
        x_pos = (int)pos.x;
        y_pos = (int)pos.y;
        gridMap.SetUnit(this, x_pos, y_pos);
    }

    private void PlaceObjectOnGrid()
    {
        Transform t = transform;
        Vector3 pos = t.position;
        x_pos = (int)pos.x;
        y_pos = (int)pos.y;
        gridMap.SetUnit(this, x_pos, y_pos);
    }

    public void RemoveObjectFromGrid()
    {
        gridMap.ClearUnit(x_pos, y_pos);
    }

    public void ChangeAnimation(string name)
    {
        if (currentAnimation != name)
        {
            animator.ResetTrigger(currentAnimation);
            currentAnimation = name;
            animator.SetTrigger(currentAnimation);
        }
    }
}

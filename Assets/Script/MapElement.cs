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

    void Start()
    {
        SetGrid();
        PlaceObjectOnGrid();
        animator = GetComponent<Animator>();
    }

    private void SetGrid()
    {
        gridMap = transform.parent.GetComponent<GridMap>();
    }

    public void MoveUnit(int targetPosX, int targetPosY)
    {
        RemoveObjectFromGrid();
        MoveTo(targetPosX, targetPosY);
        MoveObject();
        animator.Play("Blue_Infantry_Idle");
    }

    //Not working
    private void CheckMoveDirection(Vector3 targetPosition)
    {
        Vector2 direction = transform.localScale; 
        if (targetPosition.x > transform.position.x)
        {
            //direction.x *= -1;
            //transform.localScale = direction;
        }
        if (targetPosition.x < transform.position.x)
        {
            animator.Play("Blue_Infantry_Move_Horizontal");
        }
        if (targetPosition.y > transform.position.y)
        {
            animator.Play("Blue_Infantry_Move_Up");
        }
        if (targetPosition.y < transform.position.y)
        {
            animator.Play("Blue_Infantry_Move_Down");
        }
    }

    private void MoveObject()
    {
        Vector3 worldPosition = new Vector3(x_pos * 1f + 0.5f, y_pos * 1f + 0.5f, -0.5f);
        CheckMoveDirection(worldPosition);
        StartCoroutine(MoveAnimation(worldPosition));
    }
    
    IEnumerator MoveAnimation(Vector3 targetPosition)
    {
        while (transform.position.x != targetPosition.x || transform.position.y != targetPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, unitMoveSpeed * Time.deltaTime);
            StartCoroutine(Wait(5f));
        }
        yield return null;
    }

    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

    private void MoveTo(int targetPosX, int targetPosY)
    {
        gridMap.SetUnit(this, targetPosX, targetPosY);
        x_pos = targetPosX;
        y_pos = targetPosY;
    }

    private void PlaceObjectOnGrid()
    {
        Transform t = transform;
        Vector3 pos = t.position;
        int x_pos = (int)pos.x;
        int y_pos = (int)pos.y;
        gridMap.SetUnit(this, x_pos, y_pos);
    }

    private void RemoveObjectFromGrid()
    {
        gridMap.ClearUnit(x_pos, y_pos);
    }
}

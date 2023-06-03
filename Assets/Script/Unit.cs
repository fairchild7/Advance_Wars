using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int moveDistance = 3;
    public string Name;
    SpriteRenderer spriteHp;

    private void Awake()
    {

        Name = gameObject.name;

        spriteHp = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteHp.gameObject.SetActive(false);


    }
}

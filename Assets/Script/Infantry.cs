using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Infantry : MonoBehaviour, IUnit
{
    /*
    public int move { get; set; }
    public int hp { get; set; }
    public int passability { get; set; }
    public bool isMoved { get; set; }
    //public GameInformation.Unit unit { get; set; }
    public Manager manager { get; set; }
    public Animation anim { get; set; }
    public SpriteRenderer sprCapt { get; set; }
    public SpriteRenderer sprHp { get; set; }
    public _Path refPath { get; set; }
    public GameInformation gameInfo { get; set; }
    
    void Awake()
    {
        GameObject newObj = new GameObject();
        GameInformation gameInfo = new GameInformation();
        Manager manager = new Manager();
        Animation anim = new Animation();
        refPath = newObj.AddComponent<_Path>();
        //unit = GameInformation.Unit.Infantry;
        //move = gameInfo.moveRange[(int)unit];
        hp = 10;
        //passability = gameInfo.unitPassability[(int)unit];
        isMoved = false;
        sprCapt = transform.GetChild(0).GetComponent<SpriteRenderer>();
        sprHp = transform.GetChild(1).GetComponent<SpriteRenderer>();
        sprCapt.gameObject.SetActive(false);
        sprHp.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Selected();
        ClickingTest();
    }

    public void Selected()
    {
        if (Input.GetMouseButtonDown(0))
        {
            refPath.ClearArea();
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log(refPath);
                //refPath.FindArea(unit, move, gameObject.transform.position.x, gameObject.transform.position.y);
                refPath.DrawArea();
            }
        }
    }

    public void ClickingTest()
    {

    }

    public void MoveOnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (refPath.listUnitMove.Count == 1)
            {
                return;
            }
            if (refPath.listUnitMove.Count > 1)
            {
                StartCoroutine(MoveAnim());
            }
        }
    }
    public IEnumerator MoveAnim()
    {
        float delta = .15f;
        for (int i = 0; i < refPath.listUnitMove.Count; i++)
        {
            Vector2 to = refPath.listUnitMove[i];
            while (Vector2.Distance(to, gameObject.transform.position) > 0)
            {
                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, to, delta);
                yield return null;
            }
        }
        Manager.newX = (int)refPath.listUnitMove[refPath.listUnitMove.Count - 1].x;
        Manager.newY = (int)refPath.listUnitMove[refPath.listUnitMove.Count - 1].y;
    }

    public void IntToSprite()
    {
        Sprite spr = null;
        switch (hp)
        {
            case 1:
                spr = manager.one;
                break;
            case 2:
                spr = manager.two;
                break;
            case 3:
                spr = manager.three;
                break;
            case 4:
                spr = manager.four;
                break;
            case 5:
                spr = manager.five;
                break;
            case 6:
                spr = manager.six;
                break;
            case 7:
                spr = manager.seven;
                break;
            case 8:
                spr = manager.eight;
                break;
            case 9:
                spr = manager.nine;
                break;
        }
        sprHp.sprite = spr;
    }

    public void Moved()
    {
        isMoved = true;
        gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(.25f, .25f, .25f);
    }

    public void Reset()
    {
        
    }

    public bool UpdateHp()
    {
        return true;
    }*/
}

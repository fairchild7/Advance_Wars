using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : SimpleSingleton<ObjectPoolManager>
{
    private List<GameObject> pools;

    GameObject obj;
    Transform parent;

    private void Awake()
    {
        pools = new List<GameObject>();
    }

    public void OnInit(GameObject obj, Transform parent, int amount)
    {
        this.obj = obj;
        this.parent = parent;

        for (int i = 0; i < amount; i++)
        {
            AddObjectToPool();
        }
    }

    public GameObject GetObjectFromPool()
    {
        if (pools.Count == 0)
        {
            AddObjectToPool();
        }
        GameObject obj = pools[0];
        obj.SetActive(true);
        pools.RemoveAt(0);
        return obj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        pools.Add(obj);
    }

    private void AddObjectToPool()
    {
        GameObject newObj = Instantiate(obj, parent);
        newObj.SetActive(false);
        pools.Add(newObj);
    }
}

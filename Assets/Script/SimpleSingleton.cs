using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            return instance;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEnd : MonoBehaviour
{
    [SerializeField] GridManager gridManager;

    public void EndDay()
    {
        gridManager.RefreshUnit();
    }
}

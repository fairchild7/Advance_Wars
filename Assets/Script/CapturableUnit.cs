using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturableUnit : MonoBehaviour
{
    SpriteRenderer spriteCapt;

    private void Awake()
    {
        spriteCapt = transform.GetChild(1).GetComponent<SpriteRenderer>();
        spriteCapt.gameObject.SetActive(false);
    }

    public void Capturing()
    {
        spriteCapt.gameObject.SetActive(true);
    }

    public void StopCapturing()
    {
        spriteCapt.gameObject.SetActive(false);
    }
}

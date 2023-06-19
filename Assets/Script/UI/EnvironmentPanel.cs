using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;

public class EnvironmentPanel : MonoBehaviour
{
    [SerializeField] Image environmentSprite;
    [SerializeField] TextMeshProUGUI textDefense;
    [SerializeField] Sprite[] environmentSpriteList;

    EnvironmentData eData;

    private void Start()
    {
        eData = new EnvironmentData();
    }

    public void UpdateEnvironmentPanel(TerrainType type)
    {
        environmentSprite.sprite = environmentSpriteList[(int)type];
        textDefense.text = "Defense: " + eData.defenseValue[(int)type].ToString();
    }
}

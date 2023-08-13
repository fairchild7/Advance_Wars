using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIWin : UICanvas
{
    [SerializeField] TextMeshProUGUI textSpeed;
    [SerializeField] TextMeshProUGUI textPower;
    [SerializeField] TextMeshProUGUI textTechnique;

    public override void Open()
    {
        base.Open();
        textSpeed.text = "Days: " + GameController.Instance.GetCurrentDay();
        textPower.text = "Enemy destroyed: " + GameController.Instance.enemyDestroyed;
        textTechnique.text = "Building captured: " + GameController.Instance.allyBuilding.Count;
    }

    public void ButtonMenu()
    {
        Close();
        SceneManager.LoadScene("Main Menu");
    }
}

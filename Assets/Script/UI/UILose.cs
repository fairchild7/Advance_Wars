using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UILose : UICanvas
{
    [SerializeField] TextMeshProUGUI textStage;

    public override void Open()
    {
        base.Open();
        textStage.text = SceneManager.GetActiveScene().name;
    }

    public void ButtonMenu()
    {
        Close();
        SceneManager.LoadScene("Main Menu");
    }

    public void ButtonRetry()
    {
        Close();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textCurrentStage;
    [SerializeField] int maxStageAvailable;

    int currentStage;

    private void Start()
    {
        if (PlayerData.Instance.LoadCurrentStage() == 0)
        {
            PlayerData.Instance.SetCurrentStage(1);
            PlayerData.Instance.SaveCurrentStage();
        }

        if (PlayerData.Instance.LoadCurrentStage() > maxStageAvailable)
        {
            textCurrentStage.text = maxStageAvailable.ToString();
            currentStage = int.Parse(textCurrentStage.text);
        }

        textCurrentStage.text = PlayerData.Instance.LoadCurrentStage().ToString();
        currentStage = int.Parse(textCurrentStage.text);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Stage " + currentStage);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void NextButton()
    {
        if (currentStage < PlayerData.Instance.LoadCurrentStage() && currentStage < maxStageAvailable)
        {
            currentStage++;
            textCurrentStage.text = currentStage.ToString();
        }
    }

    public void PreviousButton()
    {
        if (currentStage > 1)
        {
            currentStage--;
            textCurrentStage.text = currentStage.ToString();
        }
    }
}

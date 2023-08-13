using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    #region Singleton
    private static PlayerData instance;
    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerData();
            }
            return instance;
        }
    }
    #endregion

    #region Player Current Stage
    private int currentStage = 1;

    public int GetCurrentStage()
    {
        return currentStage;
    }

    public void SetCurrentStage(int stage)
    {
        currentStage = stage;
    }

    public void SaveCurrentStage()
    {
        PlayerPrefs.SetInt("Stage", currentStage);
        PlayerPrefs.Save();
    }

    public int LoadCurrentStage()
    {
        return PlayerPrefs.GetInt("Stage");
    }
    #endregion

    #region Player Volume
    private float volume = 0;

    public float GetVolume()
    {
        return volume;
    }

    public void SetVolume(float volume)
    {
        this.volume = volume;
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    public float LoadVolume()
    {
        return PlayerPrefs.GetFloat("Volume");
    }
    #endregion
}

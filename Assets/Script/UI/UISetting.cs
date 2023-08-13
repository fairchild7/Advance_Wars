using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    public Slider soundSlider;

    private void Start()
    {
        soundSlider.value = PlayerData.Instance.LoadVolume();    
    }

    public void SoundVolume()
    {
        AudioManager.Instance.SoundVolume(soundSlider.value);
        PlayerData.Instance.SetVolume(soundSlider.value);
        PlayerData.Instance.SaveVolume();
    }
}

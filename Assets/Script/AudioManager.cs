using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SimpleSingleton<AudioManager>
{
    public AudioSource musicSound;

    public void SoundVolume(float volume)
    {
        musicSound.volume = volume;
    }
}

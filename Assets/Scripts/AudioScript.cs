using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioScript : MonoBehaviour {

    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;
	
    public void ChangeMusicMixerVolume(float volume)
    {
        musicMixer.SetFloat("volumeMusic", volume);
    }

    public void ChangeSFXMixerVolume(float volume)
    {
        sfxMixer.SetFloat("volumeSFX", volume);
    }
}

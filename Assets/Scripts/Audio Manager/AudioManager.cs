using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Apparently caused some namespace issues with system when using. not necessary anyway
//using FMOD;
using FMOD.Studio;
using FMODUnity;


public class AudioManager : MonoBehaviour
{

    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get 
        {
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    public void Awake()
    {
        #region Duplicate instance check
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        #endregion
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Play FMOD Studio event instance via passing an <see cref="FMOD.Studio.EventInstance"/>
    /// </summary>
    /// <param name="eventInstance"></param>
    public void PlayAudioByEventInstance(EventInstance eventInstance)
    {
        eventInstance.start();
    }
    /// <summary>
    /// Stops FMOD audio playback of designated <see cref="FMOD.Studio.EventInstance"/>.
    /// Stop mode needs to be designated.
    /// </summary>
    /// <param name="eventInstance"></param>
    /// <param name="_stopMode"></param>
    public void StopAudioByEventInstance(EventInstance eventInstance, STOP_MODE _stopMode)
    {
        eventInstance.stop(_stopMode);
    }
    /// <summary>
    ///Stops FMOD audio playback of designated event. stop mode is immediate.
    /// </summary>
    /// <param name="eventInstance">FMOD Event Instance to stop playback</param>
    public void StopAudioByEventInstance(EventInstance eventInstance)
    {
        eventInstance.stop(STOP_MODE.IMMEDIATE);
    }

}

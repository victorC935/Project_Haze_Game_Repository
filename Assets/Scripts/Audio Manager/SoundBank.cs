using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
//contains personalised FMOD Studio fixes/ wrappers 
using FMODhotfixes;

public class SoundBank : MonoBehaviour
{
    //FMODUnity Event references
    //This is for allocating sound events in the editor to allow designers to implement the sound system
    public List<EventInstanceWrapper> MasterEventBank;

    #region Example code
    /// <summary>
    /// Used to give example of how to create an Instance of an FMOD event  and play / stop it.
    /// use the wrapper class if needed.
    /// </summary>
    private void ExampleEventInstance()
    {

        EventInstanceWrapper exampleMusicEvent;
        //exampleMusicEvent.EventInstance.start();
        //exampleMusicEvent.EventInstance.stop(STOP_MODE.IMMEDIATE);
    }

    ///Could be used to set and validate event instances if done by loading GUID's and references via script
    //private void OnValidate()
    //{
    //    foreach (FMOD.hotfix.FMODEventWrapper arg0 in MasterEventBank)
    //    {
    //        Debug.Log("event instance triggered");
    //        arg0.SetInstanceTest();
    //    }
    //}

    #endregion

}
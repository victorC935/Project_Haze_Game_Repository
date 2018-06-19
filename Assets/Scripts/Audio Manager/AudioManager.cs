using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Apparently caused some namespace issues with system when using. not necessary anyway
//using FMOD;
using FMOD.Studio;
using FMODUnity;


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
}

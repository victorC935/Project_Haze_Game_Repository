using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

namespace FMODhotfixes
{
    /// <summary>
    /// This namespace contains any fixes that I have made to make the implementation of FMOD somewhat easier
    /// jfaulkner3.wordpress.com
    /// </summary>
    [Serializable]
    public class EventInstanceWrapper
    {
        //toggle for preventing unnecessary re- creating of an instance during runtime
        bool alreadySetEvent = false;

        //This is the event reference that unity uses to create Instances of the events (as below)
        [SerializeField]
        [EventRef]
        public string eventRefence;


        //Creates an instance of the event automatically when called.
        private EventInstance _eventInstance;
        [SerializeField]
        public EventInstance EventInstance
        {
            get
            {
                alreadySetEvent = false;
                if (!alreadySetEvent)
                {

                }
                return _eventInstance;
            }

            //shouldnt be necesarry if I do not need to change FMOD events at runtime
            //set
            //{
            //    EventInstance = value;
            //}
        }

        public void SetInstanceTest()
        {
            _eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventRefence);
        }
        
    }

    [Serializable]
    public class ParameterWrapper
    {
        FMOD.Studio.ParameterInstance parameterInstance;

        public ParameterInstance GetParam(EventInstance eventInstance, string parameterName)
        {
            ParameterInstance _parameterInstance;
            eventInstance.getParameter(parameterName, out _parameterInstance);
            return _parameterInstance;
        }
    }


}
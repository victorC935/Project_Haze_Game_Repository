﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHingeScript : MonoBehaviour {

    // Variables that are only used by the Door game object.
    #region DoorVariables   
    public bool isWooden;

    public GameObject doorKnob;
    #endregion
    // Bool variables to determine the object type.
    #region ObjectTypes     
    public bool isDoor;
    public bool isChest;
    #endregion
    // Shared variables.
    #region Shared
    public bool interacted;
    public bool isLocked;

    public Vector3 startPos;
    #endregion

    [Tooltip("If the door does not have a key, input 0")]
    public int KeyNumber; // In case the player finds a key.


    void Start()
    {
        startPos = gameObject.transform.position;
        interacted = false;
    }

    // Update is called once per frame
    void Update()
    {
        IntChk();         // Does not let the player move the door when colliding.
    }
    void IntChk()
    {
        if (!interacted)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        if (interacted)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

	public bool interacted;
    public bool isLocked;
    public bool isWooden;

    [Tooltip("If the door does not have a key, input 0")]
    public int KeyNumber; // In case the player finds a key.


	void Start () {
        interacted = false;
    }
	
	// Update is called once per frame
	void Update () {
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

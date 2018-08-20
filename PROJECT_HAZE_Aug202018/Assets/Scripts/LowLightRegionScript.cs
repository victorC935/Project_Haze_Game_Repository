using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowLightRegionScript : MonoBehaviour {

    private GameObject player;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            player.GetComponent<PlayerMovement>().isInDark = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            player.GetComponent<PlayerMovement>().isInDark = false;
        }
    }
}

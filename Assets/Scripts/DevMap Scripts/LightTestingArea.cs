using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTestingArea : MonoBehaviour {

    public GameObject MainLight;

    public GameObject player;
    public GameObject playerLight;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            MainLight.SetActive(false);
            playerLight.GetComponent<Light>().intensity = 5f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            MainLight.SetActive(true);
            playerLight.SetActive(false);
            playerLight.GetComponent<Light>().intensity = 1f;
            playerLight.GetComponent<Light>().range = 10f;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                playerLight.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                playerLight.SetActive(false);
            }
            if(Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                playerLight.GetComponent<Light>().range = playerLight.GetComponent<Light>().range + 1f;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                playerLight.GetComponent<Light>().range = playerLight.GetComponent<Light>().range -  1f;
            }
        }
    }
}

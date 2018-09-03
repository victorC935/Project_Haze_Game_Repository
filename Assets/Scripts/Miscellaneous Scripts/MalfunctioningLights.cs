using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalfunctioningLights : MonoBehaviour {

    public GameObject GameManager;

    public float intensity;
    float onDelay;
    float offDelay;

    public bool canFlicker;
    public bool canIntensify;

	// Use this for initialization
	void Start () {
        onDelay = Random.Range(0.01f, 5);
	}
	
	// Update is called once per frame
	void Update () {
        if (canFlicker)
        {
            Flicker();
        }

        onDelay = onDelay - 1 * Time.deltaTime;
        offDelay = offDelay - 1 * Time.deltaTime;
        // Will be activated when debugging.
        if (GameManager.GetComponent<DebugManager>().debugMode)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                gameObject.GetComponent<Light>().enabled = false;
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                gameObject.GetComponent<Light>().enabled = true;
            }
            if (Input.GetKeyDown(KeyCode.P) && Input.GetKeyDown(KeyCode.O))
            {
                canFlicker = !canFlicker;
            }
        }
    }
    void Intensity()
    {
        intensity = Random.Range(0.5f, 5f);
        gameObject.GetComponent<Light>().intensity = intensity;
    }
    void Flicker()
    {
        if (onDelay < 0f && offDelay > 0f)
        {
            gameObject.GetComponent<Light>().enabled = true;
            onDelay = Random.Range(offDelay + 0.15f, 2);
        }
        if (offDelay < 0f && onDelay > 0f)
        {
            gameObject.GetComponent<Light>().enabled = false;
            offDelay = Random.Range(onDelay + 0.15f, 1);
        }

        if (onDelay < -1 && offDelay < -1) { // Check if the script had skipped a tick because of framerate and fix the problem.
            onDelay = Random.Range(offDelay + 0.15f, 2);
        }
        if ((onDelay <= 0.1f || offDelay <= 0.1f) && canIntensify) // Randomizes intensity
        {
            Intensity();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class MenuManager : MonoBehaviour {

    public bool showFrameRate;
    public string frameRate;
    GameObject fpsCounter;

    // Use this for initialization
    void Start () {
        fpsCounter = GameObject.Find("fpsCounter");
    }
	
	// Update is called once per frame
	void Update () {

    }
    private void OnGUI()
    {
        if (showFrameRate) // A simple FPS counter
        {
            fpsCounter.SetActive(true);
            float fps = 1.0f / Time.deltaTime;
            string text = string.Format("{0:0.} fps", fps);
            frameRate = text;
            fpsCounter.GetComponent<Text>().text = frameRate;
        }
        if (!showFrameRate)
        {
            fpsCounter.SetActive(false); // Disables the FPS counter on demand
        }
    }
}

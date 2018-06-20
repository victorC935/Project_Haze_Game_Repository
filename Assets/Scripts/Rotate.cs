using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
    /// <summary>
    /// Just a script to make points of interest more obvious from a distance
    /// </summary>

    [Range(-5,5)][Tooltip("Wheeee!")]
    public float rotateSpd = 5; //modify speed object rotates

    [Range(1, 2)][Tooltip("Boing Boing Boing!")]
    public float bobSpd = 1;    //modify spd object bobs (up-down)

    float xPos;
    float zPos;

    private void Start()
    {
        xPos = transform.position.x;
        zPos = transform.position.z;
    }
    void Update () {
        transform.Rotate(0, rotateSpd, 0);
        transform.position = new Vector3(xPos, 0.5f +Mathf.PingPong(Time.time * 2, bobSpd), zPos);
	}
}

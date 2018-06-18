using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour {
    public float HP;
    public float Armor;
	// Use this for initialization
	void Start () {
        HP = 100f;
        Armor = 10f;
    }
	
	// Update is called once per frame
	void Update () {
        if (HP <= 0f)
        {
            Death();
        }
	}
    void Death()
    {
        Debug.Log(gameObject.name + "has died. Resetting...");
        HP = 100f;
    }
}

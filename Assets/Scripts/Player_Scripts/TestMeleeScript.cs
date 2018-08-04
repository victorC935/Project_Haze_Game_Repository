using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMeleeScript : MonoBehaviour {

    public GameObject TestWeapon;
    public GameObject PlayerCamera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        /* This part is only temporary. It should be replaced with a script that gets called when the player is either:
                A-> Looking at a weapon that is on the ground
                B-> Taking a weapon that is stuck in an enemy or an object
                C-> Equipping a weapon from the inventory */
        if (Input.GetKeyDown(KeyCode.O) && TestWeapon.GetComponent<MeleeWeaponScript>().isEquipped == false) {
            TestWeapon.GetComponent<MeleeWeaponScript>().EquipWeapon();
        }
        if (Input.GetKeyDown(KeyCode.O) && TestWeapon.GetComponent<MeleeWeaponScript>().isEquipped == true)
        {
            TestWeapon.GetComponent<MeleeWeaponScript>().UnEquipWeapon();
        }

    }
    void Attack()
    {
        // Play the animation that swings the weapon
    }
    void Block()
    {
        // Play the animation that raises the weapon to block enemy attacks from front.
        // Check if the player is looking at the enemy, then NEGATE or DECREASE the damage taken.
    }
}

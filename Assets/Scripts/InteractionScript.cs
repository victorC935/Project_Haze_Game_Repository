using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{

    // Bit shift the index of the layer (8) to get a bit mask
    int layerMask = 1 << 9; // Used for Weapons
    int interactableLayerMask = 1 << 11; // To be used for items, doors, lights etc.

    GameObject hitObject;
    public GameObject CurrentWeapon;



    void Start()
    {
        CurrentWeapon = null;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2.5f, layerMask))
        {
            if (Input.GetButtonDown("Use"))
            {
                hitObject = hit.collider.gameObject;
                if (hitObject.GetComponent<MeleeWeaponScript>().isEquipped == false)
                {
                    if (CurrentWeapon != null)
                    {
                        CurrentWeapon.GetComponent<MeleeWeaponScript>().UnEquipWeapon();
                    }
                    if (CurrentWeapon == null)
                    {
                        hitObject.GetComponent<MeleeWeaponScript>().EquipWeapon();
                        hitObject = null;
                    }
                }
            }
        }
        RaycastHit doorHit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out doorHit, 2.5f, interactableLayerMask))
        {
            if (Input.GetButton("Use")) // This is to be used for opening the door or starting the lockpicking sequence if the player has the lockpick selected.
            {
                hitObject = hit.collider.gameObject;
                if(hitObject.tag == "Door") // Check if the object has the tag Door, so it can actually access the script.
                {
                    if (!hitObject.GetComponent<DoorScript>().isLocked) // Checks if the door is not locked
                    {
                        // This part will translate horizontal mouse movement, to apply forces on the door's Rigidbody component so it will actually open.
                        hitObject.GetComponent<DoorScript>().interacted = true;
                    }
                }
            }
            if (Input.GetButtonUp("Use"))
            {
                hitObject.GetComponent<DoorScript>().interacted = false; // Lock the door in place.
            }
        }
    }
}

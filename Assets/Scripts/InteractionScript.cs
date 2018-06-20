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

    public GameObject lastDoor;


    [Range(5.0f, 10.0f)] [Header("Recommended value is 8")]
    public float doorSpeed;

    private float openSpeed;



    void Start()
    {
        CurrentWeapon = null;
        lastDoor = null;
    }

    // Update is called once per frame
    void Update()
    {
        openSpeed = Input.GetAxis("Mouse X");
        openSpeed = Mathf.Clamp(openSpeed, -0.5f, 0.5f);

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
            if (Input.GetButtonDown("Use")) // This is to be used for opening the door or starting the lockpicking sequence if the player has the lockpick selected.
            {
                hitObject = doorHit.collider.gameObject;

                if(hitObject.tag == "Door") // Check if the object has the tag Door, so it can actually access the script.
                {
                    lastDoor = hitObject;
                }
            }
        }
        if (Input.GetButtonUp("Use") && lastDoor != null)
        {
            lastDoor.GetComponent<DoorScript>().interacted = false; // Lock the door in place.
            lastDoor = null;
        }
        if (lastDoor != null) // Checks if the door is not locked
        {
            if (!lastDoor.GetComponent<DoorScript>().isLocked && Input.GetButton("Use"))
            {
                // This part will translate horizontal mouse movement, to apply forces on the door's Rigidbody component so it will actually open.
                lastDoor.GetComponent<DoorScript>().interacted = true;

                lastDoor.GetComponent<Rigidbody>().AddForce(-openSpeed *(doorSpeed * 100), 0, openSpeed *(doorSpeed * 100));    // applies force to open the door

            }
        }
    }
}

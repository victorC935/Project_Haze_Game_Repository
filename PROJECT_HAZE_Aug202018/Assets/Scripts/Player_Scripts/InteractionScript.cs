using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{

    // Bit shift the index of the layer (8) to get a bit mask
    int layerMask = 1 << 9; // Used for Weapons
    int interactableLayerMask = 1 << 11; // To be used for items, doors, lights etc.

    public GameObject player;

    public GameObject CurrentWeapon;

    private GameObject hitObject;
    public GameObject lastObject;

    [Range(5.0f, 10.0f)] [Header("Recommended value is 8")]
    public float doorSpeed;
    private float openSpeedHorizontal;
    private float openSpeedVertical;

    private Vector3 basePos;
    private Vector3 difference;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        CurrentWeapon = null;
        lastObject = null;
        doorSpeed = doorSpeed * 2;
    }

    // Update is called once per frame
    void Update()
    {

        openSpeedHorizontal = Input.GetAxis("Mouse X");
        openSpeedHorizontal = Mathf.Clamp(openSpeedHorizontal, -0.5f, 0.5f);

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
        RaycastHit objHit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out objHit, 2.5f, interactableLayerMask))
        {
            if (Input.GetButtonDown("Use")) // This is to be used for opening the door or starting the lockpicking sequence if the player has the lockpick selected.
            {
                hitObject = objHit.collider.gameObject;

                if(hitObject.tag == "HingeObject") // Check if the object has the tag Door, so it can actually access the script.
                {
                    lastObject = hitObject.transform.parent.gameObject;
                    player.GetComponent<PlayerMovement>().lockRotation = gameObject.transform.eulerAngles;
                    player.GetComponent<PlayerMovement>().lockPosition = player.transform.position;
                }
            }
        }
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out objHit, 2.5f, interactableLayerMask))
        {       // This helps, so the player does not open doors they are not facing at.

        }
        #region Door Related
        if (Input.GetButtonUp("Use") && lastObject != null && lastObject.GetComponent<ObjectHingeScript>().isDoor)
        {
            lastObject.GetComponent<ObjectHingeScript>().interacted = false; // Lock the door in place.
            lastObject = null;
        }
        if (lastObject != null) // Checks if the door is not locked
        {
            if (!lastObject.GetComponent<ObjectHingeScript>().isLocked && Input.GetButton("Use"))
            {
                // This part will translate horizontal mouse movement, to apply forces on the door's Rigidbody component so it will actually open.
                lastObject.GetComponent<ObjectHingeScript>().interacted = true;
                if (hitObject.name == "OpenFrom" && difference.x < 1)
                {
                    player.GetComponent<PlayerMovement>().canLook = false;
                    player.GetComponent<PlayerMovement>().canMove = false;
                    lastObject.GetComponent<Rigidbody>().AddForce(-openSpeedHorizontal * (doorSpeed) * 1.5f, 0, openSpeedHorizontal * (doorSpeed));    // applies force to open the door
                }
                if (hitObject.name == "OpenFrom" && difference.x > 1)
                {
                    player.GetComponent<PlayerMovement>().canLook = false;
                    player.GetComponent<PlayerMovement>().canMove = false;
                    lastObject.GetComponent<Rigidbody>().AddForce(openSpeedHorizontal * (doorSpeed) * 1.5f, 0, -openSpeedHorizontal * (doorSpeed));    // applies force to open the door
                }
                if (hitObject.name == "OpenTo" && difference.x > -1)
                {
                    lastObject.GetComponent<Rigidbody>().AddForce(openSpeedHorizontal * (doorSpeed) * 1.5f, 0, -openSpeedHorizontal * (doorSpeed));    // applies force to open the door
                    player.GetComponent<PlayerMovement>().canLook = false;
                    player.GetComponent<PlayerMovement>().canMove = false;
                }
                if (hitObject.name == "OpenTo" && difference.x < -1)
                {
                    lastObject.GetComponent<Rigidbody>().AddForce(-openSpeedHorizontal * (doorSpeed) * 1.5f, 0, openSpeedHorizontal * (doorSpeed));    // applies force to open the door
                    player.GetComponent<PlayerMovement>().canLook = false;
                    player.GetComponent<PlayerMovement>().canMove = false;
                }
            }
            difference = lastObject.GetComponent<ObjectHingeScript>().startPos - player.transform.position;
            if (lastObject.GetComponent<ObjectHingeScript>().isLocked && Input.GetButton("Use"))
            {
                // Check the keys in the inventory if they have the right code.
            }
        }
        if (lastObject == null && player.GetComponent<PlayerMovement>().canLook == false)
        {
            player.GetComponent<PlayerMovement>().canLook = true;
            player.GetComponent<PlayerMovement>().canMove = true;
        }
        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPickupScript : MonoBehaviour
{

    // Bit shift the index of the layer (8) to get a bit mask
    int layerMask = 1 << 9;

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
    }
}

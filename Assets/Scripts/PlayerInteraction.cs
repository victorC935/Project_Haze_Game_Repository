using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    #region Variables

    //TODO Create a single seperate script that is responsible for all the player control keys
    //creates the ability to change controller layout.
    public KeyCode interactKey = KeyCode.E;

    #endregion

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            InteractRay();
        }
    }

    void InteractRay()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        GameObject intPass;
        if (Physics.Raycast(ray, out hit))
        {
            print("I'm looking at " + hit.transform.name);
        }
        else
        {
            print("I'm looking at nothing!");
        }

        intPass = hit.collider.gameObject;
        EventMan.hasInteracted(intPass);
    }
}

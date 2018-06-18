using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMan : MonoBehaviour
{

    public static EventMan Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        Instance = this;
    }

    public delegate void InteractAction(GameObject interactionPass);
    public static event InteractAction OnInteract;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void hasInteracted(GameObject intPass)
    {
        Debug.Log(" EventMan: hasInteracted has being called");
        if (OnInteract != null)
        {
            OnInteract(intPass);
            Debug.Log("OnInteract event called: " + intPass);
        }
    }
}

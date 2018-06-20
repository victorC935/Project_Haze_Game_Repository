using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement instance;


    public float speed;
    public float tilt;

    public void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
            Debug.LogError("MULTIPLE GAMEMANAGERS IN SCENE");
        }
        else
        {
            instance = this;
            //decidely unnecesary
            //DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {

        MovePlayer();



    }

    // Sets rotation based on Camera
    public void SetRotation()
    {
        Vector3 arg0 = Camera.main.transform.rotation.eulerAngles;
        arg0.x = 0;
        arg0.z = 0;
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, arg0.y, this.transform.rotation.z);
    }

    void MovePlayer()
    {

        SetRotation();

        //Move player model
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = this.gameObject.transform.TransformDirection(movement) * speed;

    }
}

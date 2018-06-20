using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Variables
    public float minX = -360.0f;
    public float maxX = 360.0f;

    public float minY = -45.0f;
    public float maxY = 45.0f;

    //Initial Values. possibly too fast.
    //public float sensX = 100.0f;
    //public float sensY = 100.0f;

    public float sensX = 40.0f;
    public float sensY = 40.0f;

    float rotationY = 0.0f;
    float rotationX = 0.0f;

    //anchor to the players cam anchor
    public GameObject CamAnchor;

    //storing the initial rotation of anchor for reset
    public Quaternion intRotation;

    CursorLockMode wantedMode;
    #endregion



    public void Awake()
    {
        if (CamAnchor == null)
        {
            Debug.LogError("Camera isn't anchored to anything");
        }
        intRotation = CamAnchor.transform.rotation;

    }

    public void Start()
    {
        //Making sure the camera is aligned with starting rotation of anchor
        this.gameObject.transform.rotation = intRotation;
        SetCursorState();
    }

    void Update()
    {
        //if (!GameManager.instance.isNarrator)
        //{
        SetPos();
        RotCam();
        //}

    }


    public void RotCam()
    {
        //if (Input.GetMouseButton(0))
        //{
        rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, minY, maxY);
        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        //}
        //PlayerInteraction.instance.SetRotation(this.gameObject.transform.rotation);
    }

    //sets the camera to follow the players anchor
    public void SetPos()
    {
        this.gameObject.transform.position = CamAnchor.transform.position;
    }

    // Apply requested cursor state
    void SetCursorState()
    {
        wantedMode = CursorLockMode.Locked;
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }
}

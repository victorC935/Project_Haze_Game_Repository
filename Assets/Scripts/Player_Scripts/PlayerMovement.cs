﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables for Developers
    private CharacterController controller;

    public float gravity;

    public bool canMove;
    public Vector3 lockPosition;
    public bool canLook;
    public Vector3 lockRotation;

    private bool isStealthed;
    private bool isCrouching;

    public float moveSpeedSet;
    private float tempJumpSpeed;
    private float moveSpeed;
    private float sprintSpeed;
    private float crouchSpeed;
    [Range(1,500)]
    public float jumpForce;
    private float distToGround;
    [Header("Recommended value is 1")]
    public float distToAbove;

    public GameObject cameraGO;
    public Vector3 size;
    private Vector3 crouchSize;
    private Vector3 camDirection;
    private Vector3 moveDirection = Vector3.zero;

    #region Mouse Variables
    float yRotation;
    float xRotation;
    float currentXRotation;
    float currentYRotation;
    float yRotationV;
    float xRotationV;
    float lookSmoothnes = 0.1f;
    #endregion
    #endregion
    #region Variables for Players (Accessed by the menu script), Contains FOV and Sensitivity.
    [Range(60,90)]
    public float fieldOfView;
    [Range(1,10)]
    public float lookSensitivity;
    #endregion
    private void Start() // Setting up basic variables, checking stuff to avoid common mistakes.
    {
        yRotation = transform.eulerAngles.y;
        controller = gameObject.GetComponent<CharacterController>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
        crouchSize = size;
        moveSpeed = moveSpeedSet;
        sprintSpeed = moveSpeedSet * 2;
        crouchSpeed = moveSpeedSet / 2;
        crouchSize.y = size.y / 2;

        if (cameraGO == null)
        {
            Debug.LogError("There is no camera assigned to the PlayerMovement script.");
        }
        if (moveSpeedSet <= 0)
        {
            Debug.LogError("Incorrect move speed value!");
        }
        if(jumpForce <= 0)
        {
            Debug.LogError("Jump Force can not be <= 0!");
        }
    }
    private void Update()
    {
        if (gameObject.transform.eulerAngles.x != 0f && gameObject.transform.eulerAngles.z != 0f) 
        {
            gameObject.transform.eulerAngles = new Vector3 (0,yRotation,0);
        }// Checks if the rotation gets messed up somehow, should not happen, but its best to make sure. Happened 1 out of 10 times in testing.
        LockPlayer();
    }
    private void FixedUpdate()
    {
        Move();
        if (!isCrouching) {
            if (Input.GetButtonDown("Run") && onGround())
            {
                Sprint();
            }
            if (Input.GetButtonUp("Run"))
            {
                moveSpeed = moveSpeedSet;
            }
            if (Input.GetButtonDown("Crouch") && onGround())
            {
                isCrouching = true;
                Crouch();
            }
            if (Input.GetButtonDown("Jump") && onGround())
            {
                Jump();
            }
        }
        if (!Input.GetButton("Crouch") && canStand() && isCrouching)
        {
            isCrouching = false;
            Crouch();
        }
        if (isCrouching && canStand())
        {
            if (Input.GetButtonUp("Crouch"))
            {
                isCrouching = false;
                Crouch();
            }
            if (Input.GetButtonDown("Run") && onGround())
            {
                isCrouching = false;
                Crouch();
                Sprint();
            }
        }
    }// Checks for inputs and calls the appropriate function for them. I did it this way to avoid a big chunk of spaghetti code.
    private void Crouch()
    {
        if (isCrouching)
        {
            moveSpeed = crouchSpeed;
            gameObject.transform.localScale = crouchSize;
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        }
        if (!isCrouching)
        {
            moveSpeed = moveSpeedSet;
            gameObject.transform.localScale = size;
        }
    }
    private void Sprint()
    {
        moveSpeed = sprintSpeed;
    }// Simple code that doubles the movement speed, if stamina gets implemented, it could be expanded.
    private void Jump()
    {
        if (canMove)
        {
            if (onGround())
            {
                tempJumpSpeed = jumpForce;
            }
            if (!onGround())
            {
                // Might be used for something later, right now just leave it blank.
            }
        }
    }// Applies force to the rigidbody on the y axis.
    private void Move()
    {
        #region Player Movement
        if (canMove)
        {
            /*    float camY = cameraGO.transform.eulerAngles.y;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, camY, transform.eulerAngles.z);
                direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                direction = transform.TransformDirection(direction);
                direction *= moveSpeed; // Syncs the input with the direction of the player and the movement speed.
                float directionx = direction.x;
                float directionz = direction.z;
                Vector3 v3 = GetComponent<Rigidbody>().velocity;
                v3.x = directionx;
                v3.z = directionz;
                GetComponent<Rigidbody>().velocity = v3; */
            moveDirection.y -= (gravity * Time.fixedDeltaTime) - (tempJumpSpeed * Time.fixedDeltaTime);
            if (tempJumpSpeed > 0)
            {
                tempJumpSpeed -= gravity * Time.fixedDeltaTime;
            }
            controller.Move(moveDirection * Time.deltaTime);

            float camY = cameraGO.transform.eulerAngles.y;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, camY, transform.eulerAngles.z);

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;
            controller.Move(moveDirection * Time.deltaTime);
        }
        // Also syncs the camera rotation to the player.
        #endregion
        #region Camera Movement
        if (canLook)
        {
            yRotation += Input.GetAxis("Mouse X") * lookSensitivity;
            xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;
            xRotation = Mathf.Clamp(xRotation, -80, 100);
            currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationV, lookSmoothnes);
            currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationV, lookSmoothnes);
            cameraGO.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
        #region Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        #endregion
        #endregion
    }// Player-, Camera-, Cursor movement
    private void Stealth()
    {

    }// Empty right now, may be used later.
    private void LockPlayer()
    {
        if (canLook == false)
        {
            cameraGO.transform.eulerAngles = lockRotation;
        }
        if (canMove == false)
        {
            gameObject.transform.position = lockPosition;
        }
    }
    bool onGround()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }// Checks if the player is on the ground.
    public bool canStand()
    {
        return !Physics.Raycast(transform.position, Vector3.up, distToAbove + 0.3f);
    }// Checks if there is nothing above the player.
}
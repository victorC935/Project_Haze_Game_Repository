using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSelector : MonoBehaviour
{

    public GameObject Area1;
    public GameObject Area2;
    public GameObject Area3;
    public GameObject Area4;
    public GameObject Area5;
    public GameObject Area6;
    public GameObject Area7;
    public GameObject Area8;
    public GameObject Area9;
// Simple testing area selector to disable unused objects.
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                Area1.SetActive(false);
                Area2.SetActive(false);
                Area3.SetActive(false);
                Area4.SetActive(false);
                Area5.SetActive(false);
                Area6.SetActive(false);
                Area7.SetActive(false);
                Area8.SetActive(false);
                Area9.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Keypad1) && Area1 != null)
            {
                Area1.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Keypad2) && Area2 != null)
            {
                Area2.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Keypad3) && Area3 != null)
            {
                Area3.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Keypad4) && Area4 != null)
            {
                Area4.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Keypad5) && Area5 != null)
            {
                Area5.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Keypad6) && Area6 != null)
            {
                Area6.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Keypad7) && Area7 != null)
            {
                Area7.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Keypad8) && Area8 != null)
            {
                Area8.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Keypad9) && Area9 != null)
            {
                Area9.SetActive(true);
            }
        }
    }

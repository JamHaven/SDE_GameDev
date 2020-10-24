using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera mainCamera;
    public Camera closeUpCamera;
 

    // Update is called once per frame
    void Update()
    {
        //On button press switch camera
        if (Input.GetKeyDown(KeyCode.X))
        {
            mainCamera.enabled = !mainCamera.enabled;
            closeUpCamera.enabled = !closeUpCamera.enabled;
        }else if (Input.GetKeyDown(KeyCode.Escape)) // Not the best place, but exits the application on escape
        {
            Application.Quit();
        }
    }
}

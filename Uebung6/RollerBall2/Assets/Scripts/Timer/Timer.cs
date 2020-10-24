using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * Handles the Timer for the end of the game and restarting
 */
public class Timer : MonoBehaviour
{
    public float timeLimit = 30;
    public Text timeCounter;
    public Text centerText;
    public ScoreboardController scoreboardController;
    public Text winMessage;
    public Camera mainCamera;
    public Camera closeUpCamera;
    
    private float timeSinceStart =0;

    private bool restartFlag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If we are waiting for a restart.. go to else
        if (!restartFlag)
        {
            // If we reached the time limit
            if (timeSinceStart >= timeLimit)
            {
                timeCounter.text = "0"; //Set timer to 0 (just in case)
                closeUpCamera.enabled = false; 
                mainCamera.enabled = true; //Switch to the top-down camera
                Time.timeScale = 0; //Freez time
                restartFlag = true; //So we wait for a restart
                centerText.enabled = true; //Show restart info
                winMessage.text = "Player " + scoreboardController.GetNameOfFirstPlayer() + " won!";
                winMessage.enabled = true; //Show winner!
            }
            else
            {
                //Time goes by.... so slowly
                timeSinceStart += Time.deltaTime;
                timeCounter.text = Math.Round(timeLimit - timeSinceStart, 2).ToString(CultureInfo.InvariantCulture);
            }
        }// If we want to restart after the game ended
        else if(Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Handles the countdown and messages at the start
 */
public class CountdownTimer : MonoBehaviour
{
    public int startCountdownTime = 5; //Time in seconds before the game starts
    public Text countDownText; //The Text field the game will write the countdown
    public AudioSource backGroundMusic; //The Music to play at the start
    public Camera mainCamera; //The main camera with the top-down view
    public Camera closeUpCamera; //the close-up camera to play the game
    
    /**
     * On Enable is called very early, which we want since we freez the time immediatly
     */
    void OnEnable()
    {
        Time.timeScale = 0; //Freez time
        StartCoroutine(Countdown(startCountdownTime)); //start countdown until start
    }

    /**
     * Coroutine to handle a Countdown without the use of Time.deltatime, since it does not work with Time.timescale
     */
    IEnumerator Countdown(int duration)
    {
        //Counts down time until 0
        while (duration > 0)
        {
            countDownText.text = duration.ToString(); //Shows timer in the text on screen
            duration --;
            yield return WaitForUnscaledSeconds(1f); //note: there is no "new" keyword.
            
            if (duration == 1) //If we have one second left --> switch cameras to ease player into the play view
            {
                mainCamera.enabled = false;
                closeUpCamera.enabled = true;
            }
        }

        countDownText.enabled = false; //hide countdown text
        backGroundMusic.PlayDelayed(0); //Play music
        Time.timeScale = 1f; //Resume time
    }
    /**
     * Waiting with Time.unscaledDeltaTime, which works with Time.timescale
     */
    IEnumerator WaitForUnscaledSeconds(float dur)
    {
        var cur = 0f;
        //Time how long we wait
        while (cur < dur)
        {        
            yield return null;
            cur += Time.unscaledDeltaTime;
        }
    }
}

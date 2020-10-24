using System;
using System.Collections;
using System.Collections.Generic;
using Agent;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /**
     * Controls the Scoreboard and tracks the players scores
     */
    public class ScoreboardController : MonoBehaviour
    {
        private List<HighScoreEntry> highScoreEntries; //List of HigScoreEntries to store information about the scores and UI

        public GameObject highScoreEntryPrefab; //Prefab to spawn on the scoreboard (Position, Score, Name)
        
        // Start is called before the first frame update
        void Start()
        {
            highScoreEntries = new List<HighScoreEntry>(); //Initialize the Arraylist
        
            //Extract the RollerAgent component from the gameobject
            var rollerAgentGameObjects = GameObject.FindGameObjectsWithTag("RollerAgent");
            
            //For every player
            for (int i = 0;i < rollerAgentGameObjects.Length;i++)
            {
                //Get the right height from the background image, so we know ho much we should move new entries
                float highscoreTempalteHeight = highScoreEntryPrefab.transform.Find("background").GetComponent<RectTransform>().rect.height;

                Vector3 positionToSpawnScoreEntry = new Vector3(0,i *highscoreTempalteHeight,0); //Create a Vector where we should spawn the new Scoreentry
                
                GameObject highScoreEntry = Instantiate(highScoreEntryPrefab, positionToSpawnScoreEntry, highScoreEntryPrefab.transform.rotation); //Spawn a new highscoreEntry
            
                highScoreEntry.transform.SetParent(GameObject.FindWithTag("HighscoreEntryContainer").transform); //set the container as parent
                highScoreEntry.transform.localScale = new Vector3(1,1,1); // Scale changed due to new parent
                highScoreEntry.transform.localPosition = positionToSpawnScoreEntry; //Use Localposition so the scoreentry is alligned to the parent
            
                //Set the text entries, name, default score and default Position
                highScoreEntry.transform.Find("scoreText").GetComponent<Text>().text = rollerAgentGameObjects[i].GetComponent<RollerAgent>().GetCurrentPoints().ToString(); //Get the scoretext and set it
                highScoreEntry.transform.Find("nameText").GetComponent<Text>().text =
                    rollerAgentGameObjects[i].GetComponent<RollerAgent>().agentName;
                highScoreEntry.transform.Find("posText").GetComponent<Text>().text = (i+1).ToString();
                if (i != 0) //Remove the default Trophy
                {
                    highScoreEntry.transform.Find("trophy").GetComponent<Image>().enabled = false;
                }

                //Add the Scoreentry to the List, with the UI entry and the RollerAgent logic to track the scores
                highScoreEntries.Add(new HighScoreEntry(highScoreEntry, rollerAgentGameObjects[i].GetComponent<RollerAgent>()));
            }

        }

        // Update is called once per frame
        void Update()
        {
            highScoreEntries.Sort(new RollerAgentComparer()); //Sort the ArrayList by score (decending)
            
            //Get the right height from the background image, so we know ho much we should move new entries
            float highscoreTempalteHeight = highScoreEntryPrefab.transform.Find("background").GetComponent<RectTransform>().rect.height;
            for (int i = 0; i < highScoreEntries.Count; i++)
            {
                //Set the new values (score and position)
                highScoreEntries[i].GetHighScoreEntry().transform.Find("posText").GetComponent<Text>().text = (i+1).ToString();
                highScoreEntries[i].GetHighScoreEntry().transform.Find("scoreText").GetComponent<Text>().text = highScoreEntries[i].GetRollerAgent().GetComponent<RollerAgent>().GetCurrentPoints().ToString(); 
                
                //Set the new position of the score entry
                highScoreEntries[i].GetHighScoreEntry().transform.localPosition = new Vector3(0,-1* i*highscoreTempalteHeight,0);
                if (i != 0) //If wer are first, set the star :)
                {
                    highScoreEntries[i].GetHighScoreEntry().transform.Find("trophy").GetComponent<Image>().enabled = false;
                }
                else
                {
                    highScoreEntries[i].GetHighScoreEntry().transform.Find("trophy").GetComponent<Image>().enabled = true;
                }
            
            }
        }

        /**
        * Get the first player.
         * Error if there is no player
        */
        public string GetNameOfFirstPlayer()
        {
            if (highScoreEntries.Count > 0)
            {
                return highScoreEntries[0].GetRollerAgent().agentName;
            }

            return "Error";
        }
    
    
    }

    /**
     * IComparer Class to make sorting by scores easier
     */
    class RollerAgentComparer : IComparer<HighScoreEntry>
    {
        public int Compare(HighScoreEntry x, HighScoreEntry y)
        {
            return (new CaseInsensitiveComparer()).Compare((y).GetRollerAgent().GetCurrentPoints(), (x).GetRollerAgent().GetCurrentPoints());
        }
    }

    /**
     * Made an object from the UI element (GameObject highScoreEntry) and the scoreTracker (RollerAgent) to track
     * all information in one object
     */
    class HighScoreEntry
    {
        private GameObject highScoreEntry; //UI Element
        private RollerAgent rollerAgent; //Agent logic, saves score

        /**
         * Can only create this object if both elements are available
         */
        public HighScoreEntry(GameObject highScoreEntry, RollerAgent rollerAgent)
        {
            this.highScoreEntry = highScoreEntry;
            this.rollerAgent = rollerAgent;
        }

        //Getters
        public RollerAgent GetRollerAgent()
        {
            return rollerAgent;
        }

        public GameObject GetHighScoreEntry()
        {
            return highScoreEntry;
        }
    }
}
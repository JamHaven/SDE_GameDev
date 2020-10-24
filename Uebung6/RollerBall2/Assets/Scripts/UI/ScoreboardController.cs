using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardController : MonoBehaviour
{

   // private GameObject[] highScoreEntryList;
    private List<HighScoreEntry> highScoreEntries;

    public GameObject highScoreEntryPrefab;
    // Start is called before the first frame update
    void Start()
    {
        highScoreEntries = new List<HighScoreEntry>();
        
        //Extract the RollerAgent component from the gameobject
        var rollerAgentGameObjects = GameObject.FindGameObjectsWithTag("RollerAgent");
        for (int i = 0;i < rollerAgentGameObjects.Length;i++)
        {
            GameObject highScoreContainer = GameObject.FindGameObjectWithTag("HighscoreEntryContainer");
            float highscoreTempalteHeight = highScoreEntryPrefab.transform.Find("background").GetComponent<RectTransform>().rect.height;

            Vector3 positionToSpawnScoreEntry = new Vector3(0,highScoreContainer.transform.position.y - i *highscoreTempalteHeight,0);
            Debug.Log(positionToSpawnScoreEntry);
            GameObject highScoreEntry = Instantiate(highScoreEntryPrefab, positionToSpawnScoreEntry, highScoreEntryPrefab.transform.rotation); //Spawn a new highscoreEntry
            //highScoreEntryList[i] = highScoreEntry; //Save the newly spawned entry to modify
            highScoreEntry.transform.parent = GameObject.FindWithTag("HighscoreEntryContainer").transform; //set the container as parent
            highScoreEntry.transform.localScale = new Vector3(1,1,1); // Scale changed due to new parent
            highScoreEntry.transform.localPosition = positionToSpawnScoreEntry;
            
            highScoreEntry.transform.Find("scoreText").GetComponent<Text>().text = rollerAgentGameObjects[i].GetComponent<RollerAgent>().GetCurrentPoints().ToString(); //Get the scoretext and set it
            highScoreEntry.transform.Find("nameText").GetComponent<Text>().text =
                rollerAgentGameObjects[i].GetComponent<RollerAgent>().agentName;
            highScoreEntry.transform.Find("posText").GetComponent<Text>().text = (i+1).ToString();
            if (i != 0)
            {
                highScoreEntry.transform.Find("trophy").GetComponent<Image>().enabled = false;
            }

            highScoreEntries.Add(new HighScoreEntry(highScoreEntry, rollerAgentGameObjects[i].GetComponent<RollerAgent>()));
        }

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(highScoreEntries.ToArray()[0].GetRollerAgent().agentName);
        highScoreEntries.Sort(new RollerAgentComparer());
        Debug.Log(highScoreEntries.ToArray()[0].GetRollerAgent().agentName);
        float highscoreTempalteHeight = highScoreEntryPrefab.transform.Find("background").GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < highScoreEntries.Count; i++)
        {
            highScoreEntries[i].GetHighScoreEntry().transform.Find("posText").GetComponent<Text>().text = (i+1).ToString();
            highScoreEntries[i].GetHighScoreEntry().transform.Find("scoreText").GetComponent<Text>().text = highScoreEntries[i].GetRollerAgent().GetComponent<RollerAgent>().GetCurrentPoints().ToString(); 
            highScoreEntries[i].GetHighScoreEntry().transform.localPosition = new Vector3(0,-1* i*highscoreTempalteHeight,0);
            if (i != 0)
            {
                highScoreEntries[i].GetHighScoreEntry().transform.Find("trophy").GetComponent<Image>().enabled = false;
            }
            else
            {
                highScoreEntries[i].GetHighScoreEntry().transform.Find("trophy").GetComponent<Image>().enabled = true;
            }
            
        }
    }
    
    
}

class RollerAgentComparer : IComparer<HighScoreEntry>
{
    public int Compare(HighScoreEntry x, HighScoreEntry y)
    {
        return (new CaseInsensitiveComparer()).Compare(((HighScoreEntry)y).GetRollerAgent().GetCurrentPoints(), ((HighScoreEntry)x).GetRollerAgent().GetCurrentPoints());
    }
}

class HighScoreEntry
{
    private GameObject highScoreEntry;
    private RollerAgent rollerAgent;

    public HighScoreEntry(GameObject highScoreEntry, RollerAgent rollerAgent)
    {
        this.highScoreEntry = highScoreEntry;
        this.rollerAgent = rollerAgent;
    }

    public RollerAgent GetRollerAgent()
    {
        return rollerAgent;
    }

    public GameObject GetHighScoreEntry()
    {
        return highScoreEntry;
    }
}

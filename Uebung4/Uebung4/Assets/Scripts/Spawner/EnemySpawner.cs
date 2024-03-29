﻿using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Spawner
{
    public class EnemySpawner : MonoBehaviour
    {

        public int maxEnemyCount = 3; //maximal Enemies on screen
        public int secondsBeforeNextSpawn = 3; //Time between spawns
        public PointSystem pointSystem; //To check if the game is done
        public float spawnDelay = 3f; //timebefore first spawn
        public GameObject[] enemies; //To support different enemies, 1.0 will only offer one enemy type
    
        private readonly List<Transform> spawnerList = new List<Transform>();

        // Start is called before the first frame update
        void Start()
        {
            foreach (Transform child in transform)
            {
                spawnerList.Add(child);
            }
        
            //Registers interval spawning
            InvokeRepeating(nameof(Spawn), spawnDelay, secondsBeforeNextSpawn); 
        }
    
        /**
     * Spawns an enemy, if the maximum amount of enemies is not reached.
     * Enemy type and spawner is selected randomly.
     */
        private void Spawn()
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemyCount)
            {
                int enemyIndex = Random.Range(0, enemies.Length); //Random enemytype
                Vector2 randomPosition = spawnerList[Random.Range(0, spawnerList.Count)].position; //Random spawner
                Vector3 randomPositionVector = new Vector3(randomPosition.x, randomPosition.y, 0);
                Instantiate(enemies[enemyIndex], randomPositionVector, transform.rotation); //Spawn
            }
        }
    }
}

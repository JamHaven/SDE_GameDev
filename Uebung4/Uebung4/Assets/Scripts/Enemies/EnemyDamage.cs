using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    public bool doesDamageOnTouch;
    public int damage;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collisionObject)
    {
        if (collisionObject.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collisionObject.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}

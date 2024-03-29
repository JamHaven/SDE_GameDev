﻿using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        public int health = 100; //Health until death

        public GameObject deathEffect;

        /**
         * Called by the object that hits this game object.
         * Makes the game object loose health
         */
        public void TakeDamage(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                Die(); //if no HP left --> Die
            }
        }

        /**
         * Called when HP reach zero to remove game object
         */
        void Die()
        {
            //plays animation and destroys it after some time
            var deathEffectInstance = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(deathEffectInstance,1.5f);
            Destroy(gameObject); //removes game object (killed)
        }
    }
}

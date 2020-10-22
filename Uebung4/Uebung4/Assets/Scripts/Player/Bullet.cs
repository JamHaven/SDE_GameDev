using Enemies;
using UnityEngine;

namespace Player
{
    /**
     * Controlls a projectile behaviour.
     */
    public class Bullet : MonoBehaviour
    {
        public float speed = 20f; //Projectile speed
        public int damage = 40; //Damage on hit
        public float timeUntilDestroyed = 0.2f; //Until object self destructs --> projectile range
        public Rigidbody2D rb; //Use physics to fly! (z coordinate should be fixed)
        public GameObject impactEffect; //Effect to play on impact
        private float m_timeAlive = 0f; //How long the object is alive
    
        // Start is called before the first frame update
        void Start()
        {
            rb.velocity = transform.right * speed; //fly in the facing direction
        }

        /**
         * If projectile hits another collider.
         */
        private void OnTriggerEnter2D(Collider2D hitInfo)
        {
            Enemy enemy = hitInfo.GetComponent<Enemy>();
            //If we hit an enemy, the enemy takes damage
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            var bullet = transform;
            //Spawn impact animation
            var impactEffectInstance = Instantiate(impactEffect, bullet.position, bullet.rotation);
            Destroy(impactEffectInstance, 0.5f); //Destroys the animation after some time to free up object
            Destroy(gameObject); //Destroy the bullet on hit
            
        }

        private void Update()
        {
            m_timeAlive += Time.deltaTime; //Tracks time alive since spawn
            //Debug.Log("Time alive: "+ m_timeAlive + " - Delta Time: " + Time.deltaTime);
            if (m_timeAlive >= timeUntilDestroyed)
            {
                Destroy(gameObject); //Destroy the bullet after a certain time (range limiter)
            }
        }
    }
}

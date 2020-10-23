using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public int maxHealth = 100;
        public int currentHealth;
        public HealthBar healthBar;
        public float invulnerabilityPeriod;
        public Animator playerAnimator;
        public CharacterController2D controller;
        
        private float m_timeSinceLastGotHit;
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        private static readonly int Death = Animator.StringToHash("Death");
        private BoxCollider2D[] m_colList; //List of attached Colliders
        
        // Start is called before the first frame update
        void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            //So we don't start invulnerable
            m_timeSinceLastGotHit = invulnerabilityPeriod;
            m_colList = GetComponentsInChildren<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                TakeDamage(20);
            }
            
            //Invulnerability Timer count
            m_timeSinceLastGotHit += Time.deltaTime;
        }

        /**
         * Called when another object inflicts damage onto the character
         */
        public void TakeDamage(int damage)
        {
            if (!IsInvulnerable() && !controller.GetIsDead())
            {
                currentHealth -= damage;
                
                playerAnimator.SetTrigger(Hurt);
                if (currentHealth <= 0)
                {
                    playerAnimator.SetTrigger(Death);
                    Die();
                }
                
                
                healthBar.SetHealth(currentHealth);
                m_timeSinceLastGotHit = 0;
                Debug.Log("Player took:" + damage + " damage.");
            }
        }

        /**
         * Returns if the player is invulnerable.
         */
        private bool IsInvulnerable()
        {
            return m_timeSinceLastGotHit < invulnerabilityPeriod;
        }

        /**
         * Disable all colliders expect the "foothold" Boxcolider
         */
        private void Die()
        {
            GetComponent<CircleCollider2D>().enabled = false; //Disable the foot circle collider
            if (m_colList.Length > 0)
            {
                m_colList[0].enabled = false; //Disable the upper box collider
            }
            controller.SetIsDead(true);
        }

        /**
         * Heals the player with "health" amount of HP.
         * Object cannot be healed higher than maxhealth.
         */
        public void Heal(int health)
        {
            if (currentHealth + health > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth += health;
            }
            healthBar.SetHealth(currentHealth);
        
        }

        /**
         * Public query if the player is at maxhealth. For Health items.
         */
        public bool IsFullHealth()
        {
            return currentHealth == maxHealth;
        }
    }
}

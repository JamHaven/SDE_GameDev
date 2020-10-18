using UnityEngine;

namespace Player
{
    /**
 * Handles weapon behaviour
     * We currently have a Shooting sword (Gunblade or like Zelda)
 * TODO: different kind of weapons
 */
    public class Weapon : MonoBehaviour
    {
        public Transform firePoint; // Where to spawn the bullet at
        public GameObject bulletPrefab; // Which bullet to use
        private float m_timeSinceAttack = 0.0f; //Tracks time since we last attacked for animation purposes and attack speed
        private int   m_currentAttack = 0; // Which attack animation are we at
        private Animator m_animator; //Which animator controller to use
    
    
        private void Start()
        {
            m_animator = GetComponent<Animator>();
        }

    
        // Update is called once per frame
        void Update()
        {
        
            // Increase timer that controls attack combo
            m_timeSinceAttack += Time.deltaTime;

            //Sword attack and Sword LASER
            if (Input.GetButtonDown("Fire1") && m_timeSinceAttack > 0.25f)
            {
                m_currentAttack++;

                // Loop back to one after third attack
                if (m_currentAttack > 3)
                    m_currentAttack = 1;

                // Reset Attack combo if time since last attack is too large
                if (m_timeSinceAttack > 1.0f)
                    m_currentAttack = 1;

                // Call one of three attack animations "Attack1", "Attack2", "Attack3"
                m_animator.SetTrigger("Attack" + m_currentAttack);

                // Reset timer
                m_timeSinceAttack = 0.0f;

                Shoot(); //Shoot a energy bullet 
            }
        }

        /**
         * Shoots an energy bullet
         */
        void Shoot()
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
using UnityEngine;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 20f;
        public int damage = 40;
        public float timeUntilDestroyed = 0.2f;
        private float m_timeAlive = 0f;
        public Rigidbody2D rb;
        public GameObject impactEffect;
    
        // Start is called before the first frame update
        void Start()
        {
            rb.velocity = transform.right * speed;
        }

        private void OnTriggerEnter2D(Collider2D hitInfo)
        {
            Enemy enemy = hitInfo.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            var bullet = transform;
            Instantiate(impactEffect, bullet.position, bullet.rotation);
            Destroy(gameObject);
        }

        private void Update()
        {
            m_timeAlive += Time.deltaTime;
            Debug.Log("Time alive: "+ m_timeAlive + " - Delta Time: " + Time.deltaTime);
            if (m_timeAlive >= timeUntilDestroyed)
            {
                Destroy(gameObject);
            }
        }
    }
}

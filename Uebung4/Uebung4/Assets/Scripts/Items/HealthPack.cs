using Player;
using UnityEngine;

namespace Items
{
    public class HealthPack : MonoBehaviour
    {
        public int healPower = 40;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    
        private void OnTriggerEnter2D(Collider2D colliderObject)
        {
            PlayerHealth playerHealth = colliderObject.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                if (!playerHealth.IsFullHealth())
                {
                    playerHealth.Heal(healPower);
                    Destroy(gameObject);
                }
            }
        }
    }
}

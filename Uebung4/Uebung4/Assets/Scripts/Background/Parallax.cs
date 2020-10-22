using UnityEngine;

namespace Background
{
    /**
     * Controls the Parallax effect for the background objects
     */
    public class Parallax : MonoBehaviour
    {
        private float m_Startpos; //Position to start the effect
        public GameObject cam; //Which camera to apply the effect to
        public float parallaxEffect; //Where to start the effect

        // Start is called before the first frame update
        void Start()
        {
            m_Startpos = transform.position.x;
        }

        // Update is called once per frame
        void Update()
        {
            //Calulation of the parallex effect
            float distance = (cam.transform.position.x * parallaxEffect);
        
            //Sets the new position to the background and finalises the effect to the screen
            transform.position = new Vector3(m_Startpos + distance, transform.position.y, transform.position.z);
        }
    }
}

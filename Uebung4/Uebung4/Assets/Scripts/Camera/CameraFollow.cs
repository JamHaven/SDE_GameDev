using UnityEngine;

namespace Camera
{
    /**
     * Handles camera movement. The camera follows the player until it hits certain thresholds.
     */
    public class CameraFollow : MonoBehaviour
    {

        public GameObject followObject; //Object to follow
        public float offset; // x axis offset to adjust camera easily
        public float yThresholdLow; //How low can the camera move
        public float yThresholdHigh; //How high can the camera move
        public float xThresholdRight; //How far right can the camera move
        public float xThresholdLeft; // How far left can the camera move
        
        private Transform m_PlayerTransform; //Transform of the object to follow
        private Vector3 m_CurrentCameraPos; // Current camera Vector3
    
        // Start is called before the first frame update
        void Start()
        {
            m_PlayerTransform = followObject.transform;
            m_CurrentCameraPos = transform.position;
        }


        // Update is called once per frame
        void LateUpdate()
        {
            //Check if the camera moves inside the x axis thresholds and adjust it if not
            if (m_PlayerTransform.position.x <= xThresholdRight && m_PlayerTransform.position.x >= xThresholdLeft)
            {
                m_CurrentCameraPos.x = m_PlayerTransform.position.x;
                m_CurrentCameraPos.x += offset;
            }
            //Check if the camera moves inside the y axis thresholds and adjust it if not
            if (m_PlayerTransform.position.y >= yThresholdLow && m_PlayerTransform.position.y <= yThresholdHigh)
            {
                m_CurrentCameraPos.y = m_PlayerTransform.position.y;
            }

            //Finalize the x and y vectors by setting the transform
            transform.position = m_CurrentCameraPos;
        }

    }
}

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pathing
{
    public class FollowPath : MonoBehaviour
    {
        public enum MovementType
        {
            MoveTowards,
            LerpTowards
        }

        public MovementType movementType = MovementType.MoveTowards;
        public MovementPath pathToFollow;
        public float speed = 1;
        public float maxDistanceToGoal = .1f;

        private IEnumerator<Transform> m_PointInPath;
        
        // Start is called before the first frame update
        void Start()
        {
            if (pathToFollow == null)
            {
                Debug.LogError("Movement Path cannot be null.", gameObject);
                return;
            }

            m_PointInPath = pathToFollow.GetNextPathPoint();

            m_PointInPath.MoveNext();

            if (m_PointInPath.Current == null)
            {
                Debug.LogError("A path must have points in it to follow", gameObject);
                return;
            }

            transform.position = m_PointInPath.Current.position;

        }

        // Update is called once per frame
        void Update()
        {
            if (m_PointInPath == null || m_PointInPath.Current == null)
            {
                return;
            }

            if (movementType == MovementType.MoveTowards)
            {
                transform.position = Vector3.MoveTowards(transform.position, m_PointInPath.Current.position,
                    Time.deltaTime * speed);
            }
            
            else if(movementType == MovementType.LerpTowards)
            {
                transform.position = Vector3.Lerp(transform.position, m_PointInPath.Current.position,
                    Time.deltaTime * speed);
            }

            //Check if close enough to the point to move to the next one
            //Using sqrMagnitude instance of Vector3.Distance, since it is faster
            var distanceSquared = (transform.position - m_PointInPath.Current.position).sqrMagnitude;
            if (distanceSquared < maxDistanceToGoal * maxDistanceToGoal)
            {
                m_PointInPath.MoveNext();
            }

        }
    }
}

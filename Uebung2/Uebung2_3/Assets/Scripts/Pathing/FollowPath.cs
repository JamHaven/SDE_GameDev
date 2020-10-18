using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pathing
{
    /**
     * A gameobject moves alone a spline "MovementPath".
     * This class handles the type of movement and which points to move to.
     */
    public class FollowPath : MonoBehaviour
    {
        public enum MovementType
        {
            MoveTowards, //fluent movement
            LerpTowards //Fast first and slower till the end
        }

        public MovementType movementType = MovementType.MoveTowards; //Default movement type
        public MovementPath pathToFollow; //Spline to move along
        public float speed = 1; //Movement speed
        public float maxDistanceToGoal = .1f; //How close the obejct has to get to move to the next spline point

        private IEnumerator<Transform> m_PointInPath; //Coroutine call to handle movement
        
        // Start is called before the first frame update
        void Start()
        {
            //If no path to move alonge is defined
            if (pathToFollow == null)
            {
                Debug.LogError("Movement Path cannot be null.", gameObject);
                return;
            }

            //Setup reference to our coroutine
            m_PointInPath = pathToFollow.GetNextPathPoint();

            //Moves to the next point (first)
            m_PointInPath.MoveNext();

            //If there are no points in the spline
            if (m_PointInPath.Current == null)
            {
                Debug.LogError("A path must have points in it to follow", gameObject);
                return;
            }

            //Set the object to the first point
            transform.position = m_PointInPath.Current.position;

        }

        // Update is called once per frame
        void Update()
        {
            //If we have a path to move alone and are in position
            if (m_PointInPath == null || m_PointInPath.Current == null)
            {
                return;
            }

            //Different movement types
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

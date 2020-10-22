using System;
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
        private static readonly int AnimatorSpeed = Animator.StringToHash("Speed");
        public float maxDistanceToGoal = .1f; //How close the obejct has to get to move to the next spline point
        public Animator animator;
        private Vector3 m_PrevLoc = Vector3.zero;
        private Vector3 m_CurrentVelocity;
        private IEnumerator<Transform> m_PointInPath; //Coroutine call to handle movement
        private bool m_IsFacingRight = false;
        
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

            var currentSpeed = Time.deltaTime * speed;
            //Different movement types
            if (movementType == MovementType.MoveTowards)
            {
                transform.position = Vector3.MoveTowards(transform.position, m_PointInPath.Current.position,
                    currentSpeed);
            }
            
            else if(movementType == MovementType.LerpTowards)
            {
                transform.position = Vector3.Lerp(transform.position, m_PointInPath.Current.position,
                    currentSpeed);
                
            }
            //Feed the animator with the movement speed to trigger the walk animation
            animator.SetFloat(AnimatorSpeed, Mathf.Abs(currentSpeed));

 

            //Check if close enough to the point to move to the next one
            //Using sqrMagnitude instance of Vector3.Distance, since it is faster
            var distanceSquared = (transform.position - m_PointInPath.Current.position).sqrMagnitude;
            if (distanceSquared < maxDistanceToGoal * maxDistanceToGoal)
            {
                m_PointInPath.MoveNext();
            }

        }

        private void FixedUpdate()
        {
            var isMovingRight = IsMovingRight(); //isMovingRight() should only be called once
            //Is the object moving to the right, but not facing it? --> Flip it and face right.
            if (isMovingRight > 0  && !m_IsFacingRight)
            {
                Flip();
                m_IsFacingRight = true;
            } else if (isMovingRight < 0 && m_IsFacingRight) //Is the object moving to the left, but not facing it? --> Flip it and face left.
            {
                Flip();
                m_IsFacingRight = false;
            }

        }

        private void Flip()
        {
            // Rotates game object on the y axis
            transform.Rotate(0f, 180f, 0f);
        }

        /**
         * Find out if the object moves right, left or stands still
         * 1: Moves to the right
         * -1: Move to the left
         * 0: Stands still
         */
        private int IsMovingRight()
        {
            //Calculates the movment vector by tracking the current and previous position
            m_CurrentVelocity  = (transform.position - m_PrevLoc) / Time.deltaTime;
            //If we are moving to the right (X increases)
            if(m_CurrentVelocity.x > 0)
            {
                //Debug.Log("Moving right");
                m_PrevLoc = transform.position;
                return 1;
            } if(m_CurrentVelocity.x < 0){ //If we are moving to the left (X decreases)
                //Debug.Log("Moving left");
                m_PrevLoc = transform.position;
                return -1;
            }
            return 0; // If we are not moving at all (or calculation errors ;) )
        }
    }
}

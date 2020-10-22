using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
    /**
     * Creates a spline with a series of points and defines movement along those points.
     */
    public class MovementPath : MonoBehaviour
    {
        //Linear: Moves from first to last point and the same way back.
        //Loop: Moves from first to last point and directly to first point again.
        public enum PathTypes
        {
            Linear,
            Loop
        }
    
        public PathTypes pathType; //Which movement typ should be used
        private int m_MovementDirection = 1; //1: forward, -1: backwards
        private int m_MovingTo = 0; //To which point ID to move to (starting at 0)
        public Transform parentSpline; //The Parent of the splinePoints


        /**
         * Visualize the splines for development
         */
        private void OnDrawGizmos()
        {
            //If parent spline exists and has at least two points to move, we can't move along
            if (parentSpline == null && parentSpline.childCount < 2)
            {
                return;
            }
        
        
            //Enumerate the child points and draw lines between them. Example behaviour:
            //Start ID 1: Draws line from ID 0 to ID 1 --> moves along until last ID is reached
            for (int currentSplineIndex = 1; currentSplineIndex < parentSpline.childCount; currentSplineIndex++)
            {
                var previousSplinePoint = parentSpline.GetChild(currentSplineIndex-1).gameObject.transform
                    .position;
                var currentSplinePoint = parentSpline.GetChild(currentSplineIndex).gameObject.transform
                    .position;
                Gizmos.DrawLine(previousSplinePoint, currentSplinePoint);
            }

            //If we have a loop we need to draw an additional line back from the last to the start
            if (pathType == PathTypes.Loop)
            {
                Gizmos.DrawLine(parentSpline.GetChild(0).gameObject.transform.position,
                    parentSpline.GetChild(parentSpline.childCount - 1).gameObject.transform.position);
            }
        }

        //Coroutine - since we contine to move longer than a single frame, we have to define a coroutine
        public IEnumerator<Transform> GetNextPathPoint()
        {
            //We need a parent spline object and at least one child
            if (parentSpline == null && parentSpline.childCount < 1)
            {
                yield break; //Return
            }

            //Used for indefinte movement
            while (true)
            {
                //prevents infinite loops and continues after second call
                yield return parentSpline.GetChild(m_MovingTo);
                //Pause

                //If only one point --> exit coroutine
                if (parentSpline.childCount == 1)
                {
                    continue;
                }

                //Moves forward unless it reached the end and returns to beginning
                if (pathType == PathTypes.Linear)
                {
                    if (m_MovingTo <= 0)
                    {
                        m_MovementDirection = 1;
                    }
                
                    else if (m_MovingTo >= parentSpline.childCount - 1)
                    {
                        m_MovementDirection = -1;
                    }
                }

                //Define MoveID by moving forwards or backwards
                m_MovingTo += m_MovementDirection;

                if (pathType == PathTypes.Loop)
                {
                    //If moving past last point --> start at beginning (loop)
                    if (m_MovingTo >= parentSpline.childCount)
                    {
                        m_MovingTo = 0;
                    }
                    //If moved past first point --> start at the last (loop)
                    if (m_MovingTo < 0)
                    {
                        m_MovingTo = parentSpline.childCount - 1;
                    }
                }
            
            }
        }
    }
}

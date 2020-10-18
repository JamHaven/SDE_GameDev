using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
    public class MovementPath : MonoBehaviour
    {
        public enum PathTypes
        {
            Linear,
            Loop
        }
    
        public PathTypes pathType;
        private int m_MovementDirection = 1;
        private int m_MovingTo = 0;
        public Transform parentSpline;


        private void OnDrawGizmos()
        {
            if (parentSpline == null && parentSpline.childCount < 2)
            {
                return;
            }
        
        
            for (int currentSplineIndex = 1; currentSplineIndex < parentSpline.childCount; currentSplineIndex++)
            {
                var previousSplinePoint = parentSpline.GetChild(currentSplineIndex-1).gameObject.transform
                    .position;
                var currentSplinePoint = parentSpline.GetChild(currentSplineIndex).gameObject.transform
                    .position;
                Gizmos.DrawLine(previousSplinePoint, currentSplinePoint);
            }

            if (pathType == PathTypes.Loop)
            {
                Gizmos.DrawLine(parentSpline.GetChild(0).gameObject.transform.position,
                    parentSpline.GetChild(parentSpline.childCount - 1).gameObject.transform.position);
            }
        }

        //Coroutine
        public IEnumerator<Transform> GetNextPathPoint()
        {
            if (parentSpline == null && parentSpline.childCount < 1)
            {
                yield break;
            }

            while (true)
            {
                //prevents infinite loops
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

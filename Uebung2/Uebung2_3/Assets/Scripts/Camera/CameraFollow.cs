using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject followObject;
    private Transform m_PlayerTransform;
    public float offset;
    public float yThresholdLow;
    public float yThresholdHigh;
    public float xThresholdRight;
    public float xThresholdLeft;
    private Vector2 m_Threshold;
    private Vector3 m_CurrentCameraPos;
    
    // Start is called before the first frame update
    void Start()
    {
        m_PlayerTransform = followObject.transform;
        m_CurrentCameraPos = transform.position;
    }


    // Update is called once per frame
    void LateUpdate()
    {

        
        if (m_PlayerTransform.position.x <= xThresholdRight && m_PlayerTransform.position.x >= xThresholdLeft)
        {
            m_CurrentCameraPos.x = m_PlayerTransform.position.x;
            m_CurrentCameraPos.x += offset;
        }

        if (m_PlayerTransform.position.y >= yThresholdLow && m_PlayerTransform.position.y <= yThresholdHigh)
        {
            m_CurrentCameraPos.y = m_PlayerTransform.position.y;
        }

        transform.position = m_CurrentCameraPos;
    }

}

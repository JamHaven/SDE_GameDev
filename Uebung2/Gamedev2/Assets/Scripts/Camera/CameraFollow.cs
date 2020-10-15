using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform playerTransform;
    public float offset;
    public float yThresholdLow;
    public float yThresholdHigh;
    public float xThresholdRight;
    public float xThresholdLeft;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 currentCameraPos = transform.position;

        if (playerTransform.position.x <= xThresholdRight && playerTransform.position.x >= xThresholdLeft)
        {
            currentCameraPos.x = playerTransform.position.x;
            currentCameraPos.x += offset;
        }

        if (playerTransform.position.y >= yThresholdLow && playerTransform.position.y <= yThresholdHigh)
        {
            currentCameraPos.y = playerTransform.position.y;
        }

        transform.position = currentCameraPos;
    }
}

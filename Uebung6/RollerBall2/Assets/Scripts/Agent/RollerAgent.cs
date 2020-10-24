using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RollerAgent : Agent
{
    [FormerlySerializedAs("Target")] public Transform target;
    Rigidbody rBody;
    public float forceMultiplier = 10;
    public string agentName;
    public Text pointCounterText;
    public Text agentNameText;
    
    private float catchTimer;
    private int currentPoints;
    
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        pointCounterText.text = currentPoints.ToString();
        agentNameText.text = agentName;
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.z = actions.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.localPosition);
// Reached target
        if (distanceToTarget < 1.42f)
        {
            SetReward(1f);
            currentPoints += 1;
            pointCounterText.text = currentPoints.ToString();
            EndEpisode();
        }
        
        // Fell off platform
        if (this.transform.localPosition.y < 0)
        {
            SetReward(-1f);
            EndEpisode();
        }
    }

    private void Update()
    {
        catchTimer += Time.deltaTime;
        //Debug.Log(catchTimer);
    }

    public override void OnEpisodeBegin()
    {
        catchTimer = 0.0f;
        if (this.transform.localPosition.y < 0)
        {
            // If the Agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3( 0, 0.5f, 0);
        }

        // Move the target to a new spot
        target.localPosition = new Vector3(Random.value * 8 - 4,
            0.5f,
            Random.value * 8 - 4);
    }
    
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }

    public int GetCurrentPoints()
    {
        return currentPoints;
    }
}

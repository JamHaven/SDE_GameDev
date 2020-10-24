using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Agent
{
    /**
     * Player and NPCS Logic using ML-Agent technology.
     * NPCs are controlled via trained models and are set in the Behaviour Script
     * Also tracks scores and handles training rewards for the NPCs
     */
    public class RollerAgent : Unity.MLAgents.Agent
    {
        [FormerlySerializedAs("Target")] public Transform target; //We earn points by reaching the box
        Rigidbody rBody; //Our own rigidbody to control force
        public float forceMultiplier = 10; //How much force we apply to the rigidbody
        public string agentName; // Our name for the scoreboard
        public Text pointCounterText; // The textfield where to display our points
        public Text agentNameText; // The textfield to display our name

        private float catchTimer; // Future Release: add reward for time
        private int currentPoints; // Points achieved by collecting targets
    
        // Start is called before the first frame update
        void Start()
        {
            rBody = GetComponent<Rigidbody>();
            pointCounterText.text = currentPoints.ToString();
            agentNameText.text = agentName;
        }
    
        /**
         * What parameters does our model observe in the environment.
         * The model can react to those parameters, but not control them.
         */
        public override void CollectObservations(VectorSensor sensor)
        {
            // Target and Agent positions
            sensor.AddObservation(target.localPosition);
            sensor.AddObservation(this.transform.localPosition);

            // Agent velocity
            sensor.AddObservation(rBody.velocity.x);
            sensor.AddObservation(rBody.velocity.z);
        }

        /**
         * Handles controls and rewards for the NPC.
         * The NPC can change the controlSignals
         */
        public override void OnActionReceived(ActionBuffers actions)
        {
            //Movement
            Vector3 controlSignal = Vector3.zero;
            controlSignal.x = actions.ContinuousActions[0];
            controlSignal.z = actions.ContinuousActions[1];
            rBody.AddForce(controlSignal * forceMultiplier); //Speed
            //How close are we to the target?
            float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.localPosition);
            // Reached target
            if (distanceToTarget < 1.42f)
            {
                SetReward(1f); //Reward a point (only used for training)
                currentPoints += 1; //get a point!
                pointCounterText.text = currentPoints.ToString(); // Change point counter
                EndEpisode(); // Reset the training area
            }
        
            // Fell off platform
            if (transform.localPosition.y < 0)
            {
                SetReward(-1f); //Bad NPC, loos a point (only used for training)
                EndEpisode(); // Reset the training area
            }
        }

        private void Update()
        {
            catchTimer += Time.deltaTime; //Track time
        }

        /**
         * When a new Episode starts (at the beginning, or after "EndEpisode"
         */
        public override void OnEpisodeBegin()
        {
            catchTimer = 0.0f; // Reset timer
            //reset velocity and vectors
            if (transform.localPosition.y < 0)
            {
                // If the Agent fell, zero its momentum
                rBody.angularVelocity = Vector3.zero;
                rBody.velocity = Vector3.zero;
                transform.localPosition = new Vector3( 0, 0.5f, 0);
            }

            // Move the target to a new spot
            target.localPosition = new Vector3(Random.value * 8 - 4,
                0.5f,
                Random.value * 8 - 4);
        }
    
        /**
         * For Player control over the ball. Set by using "Heuristics Only" in the Behaviour Script
         */
        public override void Heuristic(float[] actionsOut)
        {
            actionsOut[0] = Input.GetAxis("Horizontal");
            actionsOut[1] = Input.GetAxis("Vertical");
        }

        /**
         * Get the points achieved this playthrough
         */
        public int GetCurrentPoints()
        {
            return currentPoints;
        }
    }
}

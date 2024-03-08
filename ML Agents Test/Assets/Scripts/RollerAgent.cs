
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RollerAgent: Agent
{

    Rigidbody rBody;
    public Transform Target;
    public float forceMultiplier = 10;
    private float accum_reward = 0;
    private float step_punishment = -.0000001f;
    private float previous_distance = 10000;

    // Start is called before the first frame update
    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 1f, 0);
        }

        // Move the target to a new spot
        // Target.localPosition = new Vector3(Random.value * 8 - 4, 1f, Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(transform.rotation);


        // print("Rigid body is null? " + rBody == null);
        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = new Vector3(1,0,1);
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        // Debug.Log(actionBuffers.ContinuousActions);

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // Reached target
        if (distanceToTarget < 1.42f)
        {
            print("Reached target!");
            AddReward(10000);
            EndEpisode();
            previous_distance = 10000;
            this.transform.localPosition = new Vector3(0, 1f, 0);
        }
        else
        {
            accum_reward += step_punishment;

            if(StepCount > 90000)
            {
                Debug.Log("Failed to reach goal. Reward: " + accum_reward);

                // SetReward(accum_reward);

                this.transform.localPosition = new Vector3(0, 1f, 0);
                EndEpisode();
                previous_distance = 10000;
            }
        }

        AddReward(step_punishment);

        // Penalize for distance.
        AddReward(previous_distance - distanceToTarget);

        previous_distance = distanceToTarget;
    }

    private float epsilon = 5f;
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        if(Random.value < epsilon)
        {
            continuousActionsOut[0] = Random.value;
            continuousActionsOut[1] = Random.value;

            epsilon -= .001f;
        }
        else
        {
            continuousActionsOut[0] = Input.GetAxis("Horizontal");
            continuousActionsOut[1] = Input.GetAxis("Vertical");
        }

        // Debug.Log("Horizontal input: " + Input.GetAxis("Horizontal"));
        // Debug.Log("Vertical input: " + Input.GetAxis("Vertical"));
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("MazeWall"))
        {
            // Debug.Log("Collided with wall!");
            // this.transform.localPosition = new Vector3(0, 1f, 0);
            AddReward(-5f);
/*            EndEpisode();
            previous_distance = 10000;*/
        }
    }
}


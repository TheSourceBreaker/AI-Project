using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    Agent agent;
    public Vector3 wanderTarget;
    private Vector3 force;
    private Vector3 v;

    public float wanderTolerance;
    public float wanderRadius;
    public bool isWandering;

    private void Awake()
    {
        agent = GetComponent<Agent>();
    }
    void Start()
    {
        NewTarget();
    }

    void FixedUpdate()
    {
        if (isWandering)
        {
            if (Vector3.Distance(transform.position, wanderTarget) < wanderTolerance) // If the agent is near(wanderTol) the target
            {
                NewTarget(); // change target
            }

            v = new Vector3(wanderTarget.x - transform.position.x, 0, wanderTarget.z - transform.position.z).normalized * agent.MaxVelocity;

            force = v - agent.currentVelocity;

            agent.currentVelocity += force * Time.deltaTime;
        }
    }

    void NewTarget()
    {
        wanderTarget = new Vector3(Random.Range(-wanderRadius, wanderRadius), transform.position.y, Random.Range(-wanderRadius, wanderRadius));
        // target is automatically set to a random Pos when this is called
    }

}

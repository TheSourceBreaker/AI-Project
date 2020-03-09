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

    private void Awake()
    {
        agent = GetComponent<Agent>();
    }

    void Start()
    {
        wanderTarget = new Vector3(Random.Range(-wanderRadius, wanderRadius), transform.position.y, Random.Range(-wanderRadius, wanderRadius));
        agent.MaxVelocity = 0.5f;
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, wanderTarget) < wanderTolerance) // This keeps a steady space between two objects
        {
            NewTarget();
        }

        v = new Vector3(wanderTarget.x - transform.position.x, 0, wanderTarget.z - transform.position.z).normalized;

        force = v - agent.currentVelocity;
        agent.currentVelocity += force * Time.deltaTime;
    }

    void NewTarget()
    {
        wanderTarget = new Vector3(Random.Range(-wanderRadius, wanderRadius), transform.position.y, Random.Range(-wanderRadius, wanderRadius));
    }

}

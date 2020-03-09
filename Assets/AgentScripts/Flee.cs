using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : MonoBehaviour
{
    Agent agent;
    public GameObject Target;
    public bool isFleeing;

    private Vector3 force;
    private Vector3 v;
    private void Awake()
    {
        agent = GetComponent<Agent>();
    }

    void Start()
    {
        isFleeing = false;
        agent.currentVelocity = Vector3.zero; // set current value to all zero
    }

    void FixedUpdate()
    {
        if(isFleeing == true)
        {
            v = (transform.position - Target.transform.position).normalized * 100; //calculate a vector from the agent to its target

            force = v - agent.currentVelocity; // at the start is the same thing as saying 5 - 0 = 5;

            agent.currentVelocity += force * Time.deltaTime; // then 0 go up to 5 (with a little realisim). 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            v = other.gameObject.transform.position;
        }
        else
        {
            v = Vector3.zero;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
    Agent agent;
    public GameObject Target;
    public bool isSeeking;

    private Vector3 force;
    private Vector3 v;
    private void Awake()
    {
        agent = GetComponent<Agent>();
    }

    void Start()
    {
        isSeeking = false;
        agent.currentVelocity = Vector3.zero; // set current value to all zero
    }

    void FixedUpdate()
    {
        if (isSeeking == true && Target != null)
        {
            v = (Target.transform.position - transform.position).normalized * 100; //calculate a vector from the agent to its target

            force = v - agent.currentVelocity; // at the start is the same thing as saying 5 - 0 = 5;

            agent.currentVelocity += force * Time.deltaTime; // then 0 go up to 5 (with a little realism). 
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bush")
        {
            v = other.gameObject.transform.position;
        }
        else
        {
            v = Vector3.zero;
        }
    }
}

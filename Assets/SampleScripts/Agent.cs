using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public Vector3 currentVelocity;
    public float rotationSpeed;
    public float MaxVelocity;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currentVelocity.magnitude > MaxVelocity) // this checks to see if the magn of current-speed is greater than the max-speed
        {
            currentVelocity = currentVelocity.normalized * MaxVelocity; // Normalizes it to stay within the max-speed
        }

        transform.position += currentVelocity * Time.deltaTime; // current position goes to new Current Velocity, it's like running in place

        transform.rotation = Quaternion.LookRotation(currentVelocity);
        Quaternion rotation = Quaternion.LookRotation(currentVelocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);


        currentVelocity = Vector3.zero;
    }
}

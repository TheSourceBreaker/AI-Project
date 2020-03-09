using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float speed;
    private float VerticalInput;
    private float HorizontalInput;

    private Rigidbody rb;
    public float dropForce;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(HorizontalInput * speed, rb.velocity.y, VerticalInput * speed);


        Vector3 movement = new Vector3();
        if (Input.GetKeyDown(KeyCode.A))
        {
            movement += Vector3.left;
        }
        transform.position += movement * speed * Time.deltaTime;



    }
}

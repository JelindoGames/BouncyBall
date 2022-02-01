using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float force;
    [SerializeField] float jumpForce;
    Vector3 spdLastFrame;
    Collider col;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(Input.GetAxis("Horizontal") * force * Time.deltaTime, 0f, Input.GetAxis("Vertical")) * force * Time.deltaTime);
        print("Vel: " + rb.velocity);
        print("Accel: " + (rb.velocity - spdLastFrame) * 1000);
        spdLastFrame = rb.velocity;
    }

    private void OnCollisionStay(Collision collision)
    {
        if(Input.GetAxis("Jump") > 0.005)
        {
            rb.AddForce(new Vector3(0f, Input.GetAxis("Jump") * jumpForce * Time.deltaTime, 0f));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float force;
    [SerializeField] float jumpForce;
    Vector3 spdLastFrame;
    [SerializeField] float dropForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(Input.GetAxis("Horizontal") * force * Time.deltaTime, 0f, Input.GetAxis("Vertical") * force * Time.deltaTime));
        print("Vel: " + rb.velocity);
        print("Accel: " + (rb.velocity - spdLastFrame) * 1000);
        spdLastFrame = rb.velocity;
        if (Input.GetButtonDown("Drop"))
        {
            Debug.Log("Reached");
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.down * dropForce * Time.deltaTime);
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if(Input.GetAxis("Jump") > 0.005)
        {
            rb.AddForce(new Vector3(0f, Input.GetAxis("Jump") * jumpForce * Time.deltaTime, 0f));
        }
    }
}

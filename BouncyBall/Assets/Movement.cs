using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float force;
    [SerializeField] float jumpForce;
    Vector3 spdLastFrame;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.forward * force * Time.deltaTime);
        }
        print("Vel: " + rb.velocity);
        print("Accel: " + (rb.velocity - spdLastFrame) * 1000);
        spdLastFrame = rb.velocity;

        if (Input.GetKeyUp(KeyCode.Space)) {
            rb.AddForce(Vector3.up * jumpForce * Time.deltaTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float force;
    [SerializeField] float jumpForce;
    [SerializeField] float dropForce;
    Vector3 spdLastFrame;
    Collider col;
    public PhysicMaterial bouncey;
    public PhysicMaterial defaultMat;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis("Jump") > 0.005)
        {
            gameObject.GetComponent<Collider>().material = bouncey;
        }
        else
        {
            gameObject.GetComponent<Collider>().material = defaultMat;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(0f, -1 * dropForce * Time.deltaTime, 0f), ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(new Vector3(Input.GetAxis("Horizontal") * force * Time.deltaTime, 0f, Input.GetAxis("Vertical") * force * Time.deltaTime));
        }
        print("Vel: " + rb.velocity);
        print("Accel: " + (rb.velocity - spdLastFrame) * 1000);
        spdLastFrame = rb.velocity;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Input.GetAxis("Jump") > 0.005)
        {
            rb.AddForce(new Vector3(0f, Input.GetAxis("Jump") * jumpForce * Time.deltaTime, 0f));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Speed Booster" && !collision.gameObject.GetComponent<SpeedBoost>().needsBounce)
        {
            float addedForce = collision.gameObject.GetComponent<SpeedBoost>().addedForce;
            if (collision.gameObject.GetComponent<SpeedBoost>().directionSpecifier == Vector3.zero)
            {
                rb.AddForce(new Vector3(addedForce * Input.GetAxis("Horizontal") * Time.deltaTime, 0f, addedForce * Input.GetAxis("Vertical") * Time.deltaTime), ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(new Vector3(addedForce * collision.gameObject.GetComponent<SpeedBoost>().directionSpecifier.x * Time.deltaTime,
                    addedForce * collision.gameObject.GetComponent<SpeedBoost>().directionSpecifier.y * Time.deltaTime,
                    addedForce * collision.gameObject.GetComponent<SpeedBoost>().directionSpecifier.z * Time.deltaTime), ForceMode.Impulse);
            }
        }
        else if (collision.gameObject.tag == "Speed Booster" && collision.gameObject.GetComponent<SpeedBoost>().needsBounce && rb.velocity.y >= Mathf.Abs(0.005f))
        {
            float addedForce = collision.gameObject.GetComponent<SpeedBoost>().addedForce;
            rb.AddForce(new Vector3(0f,
                    addedForce * collision.gameObject.GetComponent<SpeedBoost>().directionSpecifier.y * Time.deltaTime,
                    0f), ForceMode.Impulse);
        }

    }
}

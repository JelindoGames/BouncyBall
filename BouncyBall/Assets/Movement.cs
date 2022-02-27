using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float force;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpForceOnBounce;
    [SerializeField] float dropForce;
    [SerializeField] float origSpeedCap;
    [SerializeField] float fakeFrictionAmt;
    [SerializeField] float fakeFrictionAmtAir;
    Vector3 spdLastFrame;
    Collider col;
    public PhysicMaterial bouncey;
    public PhysicMaterial defaultMat;
    [SerializeField] bool grounded;
    [SerializeField] bool groundedForPeriod;
    [SerializeField] int framesNeededGroundedForPeriod;
    [SerializeField] bool braking;
    public SphereCollider breakCol;

    // For Perfect Bounce
    [SerializeField] float perfectBounceMaxWait;
    [SerializeField] float perfectBounceInputBuffer;
    public bool inPerfectBounceWindow;
    bool canPressPerfectBounce = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
        Physics.autoSimulation = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canPressPerfectBounce)
        {
            StartCoroutine("PerfectBounceTimer");
        }

        if (!grounded)
        {
            groundedForPeriod = false;
        }
        if (grounded && !groundedForPeriod)
        {
            StartCoroutine("WaitForGroundedTwoFrames");
        }
    }

    IEnumerator PerfectBounceTimer()
    {
        float timer = 0;
        inPerfectBounceWindow = true;
        canPressPerfectBounce = false;

        // Perfect bounce window
        while (timer < perfectBounceMaxWait)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        inPerfectBounceWindow = false;

        // Wait for it to be okay to press perfect bounce again
        while (timer < perfectBounceInputBuffer)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        canPressPerfectBounce = true;
    }

    IEnumerator WaitForGroundedTwoFrames()
    {
        int frames = 1;

        while (frames < framesNeededGroundedForPeriod)
        {
            frames++;
            yield return new WaitForEndOfFrame();
        }
        groundedForPeriod = grounded;
    }

    private void FixedUpdate()
    {
        breakCol.radius = (2.642857143E-4f * Breakable.KineticEnergy(rb)) + 0.5704761905f;

        if (Input.GetAxis("Jump") > 0.005)
        {
            gameObject.GetComponent<Collider>().material = bouncey;
        }
        else
        {
            gameObject.GetComponent<Collider>().material = defaultMat;
        }

        if (Input.GetAxis("Drop") > 0.005 && !braking)
        {
            braking = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(new Vector3(0f, -1 * dropForce * Time.deltaTime, 0f), ForceMode.Impulse);
        }
        else
        {
            Vector2 velFwd = new Vector2(rb.velocity.x, rb.velocity.z);
            rb.AddForce(new Vector3(Input.GetAxis("Horizontal") * force * Time.fixedDeltaTime, 0f, Input.GetAxis("Vertical") * force * Time.fixedDeltaTime));
            if (velFwd.magnitude > origSpeedCap)
            {
                print("velFwd.magnitude: " + velFwd.magnitude);
                float frictionAmt = grounded ? fakeFrictionAmt : fakeFrictionAmtAir;
                Vector3 newVelXZ = rb.velocity.normalized * (rb.velocity.magnitude - (frictionAmt * Time.fixedDeltaTime));
                rb.velocity = new Vector3(newVelXZ.x, rb.velocity.y, newVelXZ.z);
            }
        }

        if (grounded && inPerfectBounceWindow)
        {
            if (groundedForPeriod)
            {
                rb.AddForce(new Vector3(0f, Input.GetAxis("Jump") * jumpForce * Time.deltaTime, 0f));
            }
            else
            {
                rb.AddForce(new Vector3(0f, Input.GetAxis("Jump") * jumpForceOnBounce * Time.deltaTime, 0f));
            }
        }
        print("Vel: " + rb.velocity);
        print("Accel: " + (rb.velocity - spdLastFrame) * 1000);
        spdLastFrame = rb.velocity;
        Physics.Simulate(Time.fixedDeltaTime);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Untagged" || collision.gameObject.tag == "Speed Booster" || collision.gameObject.tag == "Breakable")
        {
            grounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.tag == "Untagged" || collision.gameObject.tag == "Speed Booster" || collision.gameObject.tag == "Breakable")
        {
            grounded = true;
            braking = false;
        }
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
        else if (collision.gameObject.tag == "Speed Booster" && collision.gameObject.GetComponent<SpeedBoost>().needsBounce && rb.velocity.y >= Mathf.Abs(1f))
        {
            Debug.Log("Y VEL: " + rb.velocity.y);
            float addedForce = collision.gameObject.GetComponent<SpeedBoost>().addedForce;
            rb.AddForce(new Vector3(0f,
                    addedForce * collision.gameObject.GetComponent<SpeedBoost>().directionSpecifier.y * Time.deltaTime,
                    0f), ForceMode.Impulse);
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Untagged" || collision.gameObject.tag == "Untagged")
        {
            grounded = true;
            braking = false;
        }
    }
}

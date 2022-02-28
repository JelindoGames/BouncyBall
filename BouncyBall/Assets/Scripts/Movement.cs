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
    [SerializeField] float ambientFrictionAmtGround;
    [SerializeField] float ambientFrictionAmtAir;
    Vector3 spdLastFrame;
    Collider col;
    public PhysicMaterial bouncey;
    public PhysicMaterial defaultMat;
    [SerializeField] bool grounded;
    [SerializeField] bool groundedForPeriod;
    [SerializeField] int framesNeededGroundedForPeriod;
    [SerializeField] bool braking;
    public SphereCollider breakCol;
    Vector3 jumpVector;
    Vector3 velLastFrame;

    [SerializeField] float regroundableTimer; // Once you leave the ground, when groundable again?
    bool regroundable = true;

    bool inBounceSequence = false;

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
        print("Vel: " + velLastFrame);
        velLastFrame = rb.velocity;
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
                float frictionAmt = grounded ? fakeFrictionAmt : fakeFrictionAmtAir;
                Vector3 newVelXZ = rb.velocity.normalized * (rb.velocity.magnitude - (frictionAmt * Time.fixedDeltaTime));
                rb.velocity = new Vector3(newVelXZ.x, rb.velocity.y, newVelXZ.z);
            }
            else
            {
                float frictionAmt = grounded ? ambientFrictionAmtGround : ambientFrictionAmtAir;
                Vector3 newVelXZ = rb.velocity.normalized * (rb.velocity.magnitude - (frictionAmt * Time.fixedDeltaTime));
                rb.velocity = new Vector3(newVelXZ.x, rb.velocity.y, newVelXZ.z);
            }
        }

        if (inPerfectBounceWindow)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.white, 0.5f);
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }

        if (grounded && !Input.GetKey(KeyCode.Space))
        {
            inBounceSequence = false;
        }

        if (grounded && inPerfectBounceWindow)
        {
            if (!inBounceSequence)
            {
                rb.AddForce(jumpVector * jumpForce);
                SetGrounded(false);
                print("JUMP: JUMP");
            }
            else
            {
                rb.AddForce(jumpVector * jumpForceOnBounce);
                SetGrounded(false);
                print("JUMP: BOUNCE");
            }

            inBounceSequence = true;
        }
        spdLastFrame = rb.velocity;
        Physics.Simulate(Time.fixedDeltaTime);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Untagged" || collision.gameObject.tag == "Speed Booster" || collision.gameObject.tag == "Breakable")
        {
            SetGrounded(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Untagged" || collision.gameObject.tag == "Speed Booster" || collision.gameObject.tag == "Breakable")
        {
            SetGrounded(true);
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
            float addedForce = collision.gameObject.GetComponent<SpeedBoost>().addedForce;
            rb.AddForce(new Vector3(0f,
                    addedForce * collision.gameObject.GetComponent<SpeedBoost>().directionSpecifier.y * Time.deltaTime,
                    0f), ForceMode.Impulse);
        }

        jumpVector = collision.GetContact(0).normal;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Untagged")
        {
            SetGrounded(true);
            print("Normal: " + collision.GetContact(0).normal);
            jumpVector = collision.GetContact(0).normal;
            braking = false;
        }
    }

    void SetGrounded(bool state)
    {
        if (state == true && !regroundable)
        {
            return;
        }
        grounded = state;
        if (state == false)
        {
            StartCoroutine("HandleRegroundableTimer");
        }
    }

    IEnumerator HandleRegroundableTimer()
    {
        float timer = 0;
        regroundable = false;

        while (timer < regroundableTimer)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        regroundable = true;
    }
}

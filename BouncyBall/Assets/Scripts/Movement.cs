using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    enum State
    {
        DropSuspension,
        DropPhysical,
        Bouncing,
        Rolling,
        RollBraking
    }

    State state = State.Rolling;

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
    [SerializeField] float dropSuspensionLength;
    [SerializeField] Transform cam;

    [SerializeField] PhysicMaterial bouncey;
    [SerializeField] PhysicMaterial defaultMat;

    [SerializeField] GameObject perfectBounceParticles;
    [SerializeField] List<ParticleSystem> dropParticles;

    [SerializeField] Text speedText;

    MovementInputHelper movementInputHelper;
    SpecialMovementInteractions specialMovementInteractions;
    BounceSoundPlayer bounceSoundPlayer;
    bool hasStartedDropSuspension = false;
    bool hasStartedDropPhysical = false;

    public SphereCollider breakCol;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        movementInputHelper = GetComponent<MovementInputHelper>();
        specialMovementInteractions = GetComponent<SpecialMovementInteractions>();
        bounceSoundPlayer = GetComponent<BounceSoundPlayer>();
        Physics.autoSimulation = false;
        breakCol.radius = (4.642857143E-4f * Breakable.KineticEnergy(rb)) + 2f; // What?
    }

    void Update()
    {
        if (!LevelManager.levelPlaying) return;
        speedText.text = "Speed: " + (int)rb.velocity.magnitude;
    }

    void FixedUpdate()
    {
        if (!LevelManager.levelPlaying) return;
        GetComponent<Collider>().material = movementInputHelper.inBounceWindow ? bouncey : defaultMat;
        print(state.ToString());

        switch (state)
        {
            case State.Rolling:
                HandleRollState();
                break;
            case State.RollBraking:
                HandleRollBrakingState();
                break;
            case State.Bouncing:
                HandleBounceState();
                break;
            case State.DropSuspension:
                HandleDropSuspensionState();
                break;
            case State.DropPhysical:
                HandleDropPhysicalState();
                break;
        }

        Physics.Simulate(Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathPlane"))
        {
            StartCoroutine(FindObjectOfType<LevelManager>().PlayerHitsDeathPlane());
            rb.velocity = Vector3.zero;
        }
        else if (other.CompareTag("LevelEnd"))
        {
            FindObjectOfType<LevelManager>().PlayerHitsLevelEnd(other.gameObject, gameObject);
        }
    }

    void HandleRollState()
    {
        if (!movementInputHelper.grounded)
        {
            state = State.Bouncing;
            return;
        }
        if (Input.GetAxis("Drop") > 0.005f)
        {
            state = State.RollBraking;
            return;
        }
        HandleXZMovement();
        if (movementInputHelper.inPerfectBounceWindow)
        {
            rb.AddForce(specialMovementInteractions.jumpVector * jumpForce);
            bounceSoundPlayer.Play(BounceSoundPlayer.BounceType.Jump);
            movementInputHelper.ForceUngrounded();
            state = State.Bouncing;
            print("Hi3");
        }
    }

    void HandleRollBrakingState()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
        state = State.Rolling;
    }

    void HandleBounceState()
    {
        if (movementInputHelper.groundedForPeriod && !Input.GetKey(KeyCode.Space))
        {
            state = State.Rolling;
            return;
        }
        if (Input.GetAxis("Drop") > 0.005f)
        {
            state = State.DropSuspension;
        }
        if (movementInputHelper.inPerfectBounceWindow && movementInputHelper.grounded)
        {
            bounceSoundPlayer.Play(BounceSoundPlayer.BounceType.PerfectBounce);
            GameObject particles = Instantiate(perfectBounceParticles);
            particles.transform.position = transform.position;
            rb.AddForce(specialMovementInteractions.jumpVector * jumpForceOnBounce);
            movementInputHelper.ForceUngrounded();
            print("Hi1");
        }
        else if (movementInputHelper.inBounceWindow && movementInputHelper.grounded)
        {
            bounceSoundPlayer.Play(BounceSoundPlayer.BounceType.Bounce);
            movementInputHelper.ForceUngrounded();
            print("Hi2");
        }
        HandleXZMovement();
    }

    void HandleDropSuspensionState()
    {
        if (!hasStartedDropSuspension) // Run this only in the first frame of the state
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            foreach (ParticleSystem p in dropParticles)
            {
                p.Play();
            }
            rb.constraints = RigidbodyConstraints.FreezeAll;
            hasStartedDropSuspension = true;
            Invoke("StartDropPhysical", dropSuspensionLength);
        }
    }

    void StartDropPhysical()
    {
        state = State.DropPhysical;
        hasStartedDropSuspension = false;
    } 

    void HandleDropPhysicalState()
    {
        if (!hasStartedDropPhysical) // Run this only in the first frame of the state
        {
            foreach (ParticleSystem p in dropParticles)
            {
                p.Stop();
            }
            hasStartedDropPhysical = true;
            Drop();
        }
        if (movementInputHelper.grounded)
        {
            state = State.Rolling;
        }
    }

    void Drop()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(new Vector3(0f, -1 * dropForce, 0f), ForceMode.Impulse);
    }

    void HandleXZMovement()
    {
        Vector3 camFwdXZ = cam.transform.forward;
        camFwdXZ.y = 0;
        camFwdXZ = camFwdXZ.normalized;
        Vector3 camRightXZ = cam.transform.right;
        camRightXZ.y = 0;
        camRightXZ = camRightXZ.normalized;
        Vector3 forceFwd = camFwdXZ * Input.GetAxis("Vertical") * force * Time.fixedDeltaTime;
        Vector3 forceRight = camRightXZ * Input.GetAxis("Horizontal") * force * Time.fixedDeltaTime;
        rb.AddForce(forceFwd + forceRight);
        Vector2 velFwd = new Vector2(rb.velocity.x, rb.velocity.z);
        if (velFwd.magnitude > origSpeedCap)
        {
            float frictionAmt = movementInputHelper.grounded ? fakeFrictionAmt : fakeFrictionAmtAir;
            Vector3 newVelXZ = rb.velocity.normalized * (rb.velocity.magnitude - (frictionAmt * Time.fixedDeltaTime));
            rb.velocity = new Vector3(newVelXZ.x, rb.velocity.y, newVelXZ.z);
        }
        else
        {
            float frictionAmt = movementInputHelper.grounded ? ambientFrictionAmtGround : ambientFrictionAmtAir;
            Vector3 newVelXZ = rb.velocity.normalized * (rb.velocity.magnitude - (frictionAmt * Time.fixedDeltaTime));
            rb.velocity = new Vector3(newVelXZ.x, rb.velocity.y, newVelXZ.z);
        }
    }
}

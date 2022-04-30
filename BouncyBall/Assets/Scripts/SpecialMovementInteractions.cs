using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles all of the special interactions that can take place
// during movement, like hitting a speed booster.
public class SpecialMovementInteractions : MonoBehaviour
{
    public Vector3 jumpVector { get; private set; }
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!LevelManager.levelPlaying) return;

        if (collision.gameObject.tag == "Speed Booster" && !collision.gameObject.GetComponent<SpeedBoost>().needsBounce)
        {
            HandleSpeedBoosterNoBounce(collision);
        }
        else if (collision.gameObject.tag == "Speed Booster" && collision.gameObject.GetComponent<SpeedBoost>().needsBounce && rb.velocity.y >= Mathf.Abs(1f))
        {
            HandleSpeedBoosterBounce(collision);
        }

        jumpVector = collision.GetContact(0).normal;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!LevelManager.levelPlaying) return;

        if (collision.gameObject.tag == "Untagged")
        {
            jumpVector = collision.GetContact(0).normal;
        }
    }

    void HandleSpeedBoosterNoBounce(Collision collision)
    {
        SpeedBoost sb = collision.gameObject.GetComponent<SpeedBoost>();
        float addedForce = sb.addedForce;
        Vector3 specifiedDir = sb.directionSpecifier;
        if (specifiedDir == Vector3.zero)
        {
            Vector3 force = new Vector3(addedForce * Input.GetAxis("Horizontal"), 0f, addedForce * Input.GetAxis("Vertical"));
            rb.AddForce(force * Time.deltaTime, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(addedForce * specifiedDir, ForceMode.Impulse);
        }
    }

    void HandleSpeedBoosterBounce(Collision collision)
    {
        float addedForce = collision.gameObject.GetComponent<SpeedBoost>().addedForce;
        Vector3 specifiedDir = collision.gameObject.GetComponent<SpeedBoost>().directionSpecifier;
        rb.AddForce(Vector3.up * addedForce * specifiedDir.y * Time.deltaTime,
            ForceMode.Impulse);
    }
}

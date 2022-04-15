using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoeScript : MonoBehaviour
{
    float startingHeight;
    bool movingUp = false;
    Rigidbody rb;
    private LevelManager levelMan;

    public float movementSpeedUp;
    public float movementSpeedDown;
    public float level;

    void Start()
    {
        startingHeight = transform.position.y;
        rb = GetComponent<Rigidbody>();
        levelMan = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
    }

    void FixedUpdate()
    {
        if (levelMan.currentLevelIdx == level) {
            if (transform.position.y >= startingHeight || !movingUp)
            {
                rb.AddForce(Vector3.down * movementSpeedDown, ForceMode.Force);
                movingUp = false;
            }
            else if (movingUp)
            {
                rb.AddForce(Vector3.up * movementSpeedUp, ForceMode.Force);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        movingUp = true;
    }
}

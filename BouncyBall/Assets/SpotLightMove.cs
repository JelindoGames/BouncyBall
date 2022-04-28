using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Oscillates a spotlight from right to left.
/// Movement can be stopped or started from another script / event.
/// </summary>
public class SpotLightMove : MonoBehaviour
{
    [SerializeField] bool moveOnStart;
    // How far the left/right endpoint is from the starting point.
    // (Starting point is in the middle)
    [SerializeField] float leftRightAmplitude;
    [SerializeField] float speed;
    Vector3 origPos;
    float timer;
    bool timerActive;

    void Start()
    {
        origPos = transform.position;
        if (moveOnStart)
        {
            timerActive = true; // Start moving;
        }   
    }

    void Update()
    {
        if (timerActive)
        {
            timer += Time.deltaTime;
        }
        float sinValue = Mathf.Sin(timer * speed * Mathf.PI) * leftRightAmplitude;
        transform.position = origPos + Vector3.right * sinValue;
    }

    public void StartMove()
    {
        timerActive = true;
    }

    public void StopMove()
    {
        timerActive = false;
    }
}

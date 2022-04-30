using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves an object smoothly from one position to another
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 distDiff;
    [SerializeField] float speed;
    Vector3 init;
    Vector3 goal;

    void Start()
    {
        init = transform.position;
        goal = init + distDiff;
    }

    void Update()
    {
        float sinValue = (Mathf.Sin(Time.time * speed) + 1) / 2;
        transform.position = Vector3.Lerp(init, goal, sinValue);
    }
}

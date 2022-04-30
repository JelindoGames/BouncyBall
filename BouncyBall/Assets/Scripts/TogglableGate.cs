using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Script for a gate that, when touched, can do something
// (including moving up to allow the player in)
// NOTE: This script allows the gate to move up infinitely if you touch
// it enough. We thought this was fun, so we kept it!
public class TogglableGate : MonoBehaviour
{
    [SerializeField] Vector3 distToMove;
    [SerializeField] float time;
    [SerializeField] UnityEvent onTouched;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onTouched.Invoke(); // Unity Event which can cause anything you want to happen
        }
    }

    public void MoveUp()
    {
        StartCoroutine("Move");
    }

    IEnumerator Move()
    {
        // Move upwards (or I guess any direction really) by some amount.
        Vector3 origPos = transform.position;
        float timer = 0f;
        while (timer < time)
        {
            transform.position = Vector3.Lerp(origPos, origPos + distToMove, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = origPos + distToMove;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Explodes this object in a random direction.
public class RandomExplosion : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float forceRadius;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Must have a RB");
        }
    }

    private void OnEnable()
    {
        rb.AddForce(new Vector3(Random.Range(-forceRadius, forceRadius), Random.Range(-forceRadius, forceRadius), Random.Range(-forceRadius, forceRadius)), ForceMode.Impulse);
    }
}

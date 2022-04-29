using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpiritShot : MonoBehaviour
{
    public VisualEffect shot;
    public VisualEffect explosion;
    public float speed;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        shot.enabled = true;
        explosion.enabled = false;
        rb = GetComponent<Rigidbody>();
        Invoke("Destruction", 15);
    }

    // Update is called once per frame
    void Update()
    {
        if (shot.enabled)
            rb.AddForce(Vector3.up * speed * Time.deltaTime, ForceMode.Impulse);
    }

    void Destruction()
    {
        Destroy(gameObject);
    }
}

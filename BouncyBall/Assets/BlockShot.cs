using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockShot : MonoBehaviour
{
    public float speed = 5;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("Destruction", 10f);
    }

    private void Update()
    {
        Debug.Log("Movement");
        rb.AddForce(transform.forward * speed * Time.deltaTime);
    }

    public void Destruction()
    {
        Destroy(gameObject);
    }
}

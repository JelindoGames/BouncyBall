using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockShot : MonoBehaviour
{
    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destruction", 10f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("Movement");
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    public void Destruction()
    {
        Destroy(gameObject);
    }
}

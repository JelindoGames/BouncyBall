using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public float breakEnergy;

    public AudioClip breakingSFX;

    public ParticleSystem particalEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Crush")
        {
            Debug.Log("Player Energy: " + KineticEnergy(other.gameObject.transform.parent.gameObject.GetComponent<Rigidbody>()));
        }
        if (other.gameObject.tag == "Crush" && KineticEnergy(other.gameObject.transform.parent.gameObject.GetComponent<Rigidbody>()) >= breakEnergy)
        {
            particalEffect.Play();
            AudioSource.PlayClipAtPoint(breakingSFX, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }

    public static float KineticEnergy(Rigidbody rb)
    {
        // mass in kg, velocity in meters per second, result is joules
        return 0.5f * rb.mass * rb.velocity.sqrMagnitude;
    }
}

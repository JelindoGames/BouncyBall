using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Breakable : MonoBehaviour
{
    public float breakEnergy;
    [SerializeField] UnityEvent onBroken;

    public AudioClip breakingSFX;
    public ParticleSystem particalEffect;
    public bool dontDestroy = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Crush")
        {
            Debug.Log("Player Energy: " + KineticEnergy(other.gameObject.transform.parent.gameObject.GetComponent<Rigidbody>()));
        }
        if (other.gameObject.tag == "Crush" && KineticEnergy(other.gameObject.transform.parent.gameObject.GetComponent<Rigidbody>()) >= breakEnergy)
        {
            if (particalEffect != null)
            {
                particalEffect.Play();
            }
            if (breakingSFX != null)
            {
                AudioSource.PlayClipAtPoint(breakingSFX, Camera.main.transform.position);
            }
            onBroken.Invoke();
            if (!dontDestroy)
                Destroy(gameObject);
        }
    }

    public static float KineticEnergy(Rigidbody rb)
    {
        // mass in kg, velocity in meters per second, result is joules
        return 0.5f * rb.mass * rb.velocity.sqrMagnitude;
    }
}

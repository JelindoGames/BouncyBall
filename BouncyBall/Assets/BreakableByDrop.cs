using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// If the player touches the trigger of this gameObject while in drop
// mode, destroy this object
public class BreakableByDrop : MonoBehaviour
{
    public UnityEvent onDestroy;
    [SerializeField] GameObject audioPlayer;
    [SerializeField] AudioClip destroyedSound;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Movement m = other.gameObject.GetComponent<Movement>();
            if (m.currentState() == Movement.State.DropPhysical)
            {
                AudioSource audio = Instantiate(audioPlayer, Camera.main.transform.position, Quaternion.identity).GetComponent<AudioSource>();
                audio.clip = destroyedSound;
                audio.Play();
                onDestroy.Invoke();
                Destroy(gameObject);
            }
        }
    }
}

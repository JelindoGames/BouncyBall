using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonBehaviour : MonoBehaviour
{
    public UnityEvent onPress;

    public Transform button;

    //[SerializeField] GameObject audioPlayer;
    //[SerializeField] AudioClip pressedSound;

    private bool isPressed = false;

    private void Awake()
    {
        if (!button)
        {
            button = transform.GetChild(0);
        }
    }

    void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player") && !isPressed)
        {
            
            Movement m = other.gameObject.GetComponent<Movement>();
            Debug.Log("State" + m.currentState());
            if (m.currentState() == Movement.State.DropPhysical)
            {
                Debug.Log("Here 2");
                //AudioSource audio = Instantiate(audioPlayer, Camera.main.transform.position, Quaternion.identity).GetComponent<AudioSource>();
                //audio.clip = pressedSound;
                //audio.Play();
                onPress.Invoke();
                ButtonPress();
                
            }
        }
    }

    void ButtonPress()
    {
        if (!isPressed)
        {
            button.position -= new Vector3(0f, .5f, 0f);
            isPressed = true;
        }
        
    }
}

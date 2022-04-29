using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonBehaviour : MonoBehaviour
{
    public UnityEvent onPress;

    public Transform button;

    Vector3 origPos;

    //[SerializeField] GameObject audioPlayer;
    //[SerializeField] AudioClip pressedSound;

    public bool isPressed = false;

    private void Awake()
    {
        if (!button)
        {
            button = transform.GetChild(0);
        }
        origPos = button.position;
    }

    void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player") && !isPressed)
        {
            
            Movement m = other.gameObject.GetComponent<Movement>();
            if (m.currentState() == Movement.State.DropPhysical)
            {
                //AudioSource audio = Instantiate(audioPlayer, Camera.main.transform.position, Quaternion.identity).GetComponent<AudioSource>();
                //audio.clip = pressedSound;
                //audio.Play();
                onPress.Invoke();
                ButtonPress();
                
            }
        }
    }

    private void Update()
    {
        if (!isPressed)
        {
            button.position = origPos;
        }
        else
        {
            button.position = origPos - new Vector3(0f, .5f, 0f);
        }
    }

    void ButtonPress()
    {
        if (!isPressed)
        {
            isPressed = true;
        }
        
    }
}

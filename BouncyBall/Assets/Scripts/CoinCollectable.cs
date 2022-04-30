using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows a coin to be collected. When collected,
// tells the level manager about it.
public class CoinCollectable : MonoBehaviour
{
    LevelManager lm;
    Animator anim;
    bool collected = false;

    private void Start()
    {
        lm = FindObjectOfType<LevelManager>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            lm.CoinCollected();
            anim.SetTrigger("Collected"); // Animation will set scale to nothing
            Destroy(gameObject, 2);
        }
    }
}

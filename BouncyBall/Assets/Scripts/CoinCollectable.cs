using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectable : MonoBehaviour
{
    LevelManager lm;

    private void Start()
    {
        lm = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lm.CoinCollected();
            Destroy(gameObject);
        }
    }
}

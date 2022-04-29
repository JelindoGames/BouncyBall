using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gives the stick bug the ability to totally wreck the player
public class StickBugTerminator : MonoBehaviour
{
    [SerializeField] GameObject stickbugText;
    [SerializeField] float stickbugTextLength;
    [SerializeField] float wreckPower;

    private void Start()
    {
        stickbugText.SetActive(false);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 posDiffDir = (other.gameObject.transform.position - transform.position).normalized;
            Vector3 wreckVector = new Vector3(posDiffDir.x, 1, posDiffDir.z) * wreckPower;
            other.gameObject.GetComponent<Rigidbody>().AddForce(wreckVector, ForceMode.Impulse);
            StartCoroutine("GetStickBugged");
        }
    }

    IEnumerator GetStickBugged()
    {
        stickbugText.SetActive(true);
        yield return new WaitForSeconds(stickbugTextLength);
        stickbugText.SetActive(false);
    }
}

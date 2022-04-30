using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TogglableGate : MonoBehaviour
{
    [SerializeField] Vector3 distToMove;
    [SerializeField] float time;
    [SerializeField] UnityEvent onTouched;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onTouched.Invoke();
        }
    }

    public void MoveUp()
    {
        StartCoroutine("Move");
    }

    IEnumerator Move()
    {
        Vector3 origPos = transform.position;
        float timer = 0f;
        while (timer < time)
        {
            transform.position = Vector3.Lerp(origPos, origPos + distToMove, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = origPos + distToMove;
    }
}

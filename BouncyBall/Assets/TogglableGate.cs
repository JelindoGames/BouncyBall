using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglableGate : MonoBehaviour
{
    [SerializeField] Vector3 distToMove;
    [SerializeField] float time;

    public void OnActivate()
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

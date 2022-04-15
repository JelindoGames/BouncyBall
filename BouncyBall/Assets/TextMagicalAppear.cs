using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMagicalAppear : MonoBehaviour
{
    Transform player;
    [SerializeField] float distanceMax;
    [SerializeField] float distanceMin;
    TextMesh textmesh;

    private void Start()
    {
        textmesh = GetComponent<TextMesh>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        textmesh.color = new Color(0, 0, 0, Mathf.InverseLerp(distanceMax, distanceMin, Vector3.Distance(player.position, transform.position)));
    }
}

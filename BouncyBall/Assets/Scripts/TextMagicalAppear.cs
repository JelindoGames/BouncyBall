using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMagicalAppear : MonoBehaviour
{
    Transform player;
    [SerializeField] float distanceMax;
    [SerializeField] float distanceMin;
    TextMesh textmesh;
    Color origColor;

    private void Start()
    {
        textmesh = GetComponent<TextMesh>();
        origColor = textmesh.color;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Make the color the original color, except the transparency should
        // depend on the distance to the player.
        textmesh.color = origColor - new Color(0, 0, 0, 1) +
            new Color(0, 0, 0, Mathf.InverseLerp(distanceMax, distanceMin, Vector3.Distance(player.position, transform.position)));
    }
}

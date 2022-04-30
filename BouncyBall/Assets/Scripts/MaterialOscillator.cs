using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves a material from one color to another.
public class MaterialOscillator : MonoBehaviour
{
    Renderer rend;
    [SerializeField] Color color1;
    [SerializeField] Color color2;
    [SerializeField] float speed;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        float sinValue = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        rend.material.color = Color.Lerp(color1, color2, sinValue);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player;
    public Vector3 offsetPos;
    public Quaternion offsetRot;
    public Vector3 offsetSca;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.position = player.transform.position + offsetPos;
        transform.rotation = offsetRot;
        transform.localScale = offsetSca;
    }
}

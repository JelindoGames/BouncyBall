using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Light))]
public class LightDetection : MonoBehaviour
{
    public static event Action<GameObject> PlayerIsInLight;

    public AudioClip deathFalling;

    public LevelManager levelManager;

    private string playerTag = "Player";

    private Light light;
    private GameObject player;

    private bool haveEntered = false;

    void Start()
    {
        this.light = this.GetComponent<Light>();
        this.light.type = LightType.Spot;
        this.player = GameObject.FindGameObjectWithTag(this.playerTag);
    }

    void Update()
    {
        if (this.IsPlayerInLight() && !haveEntered)
        {
            haveEntered = true;
            //AudioSource.PlayClipAtPoint(deathFalling, Camera.main.transform.position);
            levelManager.PlayerHitsDeathPlane();
            GameObject.FindGameObjectWithTag(playerTag).GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private float LightHalfAngle()
    {
        return this.light.spotAngle / 2;
    }

    private Vector3 ToPlayer()
    {
        return this.player.transform.position - this.transform.position;
    }

    private bool IsPlayerInLight()
    {
        //Debug.Log("In cone: " + this.IsPlayerInCone() + ", In light: " + this.IsPlayerInLineOfSight());
        Debug.DrawLine(this.transform.position, this.player.transform.position);
        return this.IsPlayerInCone() || this.IsPlayerInLineOfSight();
    }

    private bool IsPlayerInCone()
    {
        float distanceToPlayer = this.ToPlayer().magnitude;
        float angleToPlayer = Vector3.Angle(this.transform.forward, this.ToPlayer());
        return angleToPlayer <= this.LightHalfAngle() && distanceToPlayer <= this.light.range;
    }

    private bool IsPlayerInLineOfSight()
    {
        Ray ray = new Ray(this.transform.position, this.ToPlayer());
        if (Physics.Raycast(this.transform.position, this.ToPlayer(), out RaycastHit hit, this.light.range, ~LayerMask.GetMask()))
        {
            return hit.collider.CompareTag(this.playerTag);
        }
        return false;
    }

    private void BroadcastPlayerSpotted()
    {
        if (PlayerIsInLight != null)
        {
            PlayerIsInLight.Invoke(this.gameObject);
        }
    }
}

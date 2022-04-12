using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBoss : MonoBehaviour
{
    public enum Phase
    {
        ORIGIN, BLOCKS, SHOT, STUN
    }

    public List<GameObject> blocks;
    public GameObject middleBlock;
    public GameObject shot;
    public Collider top;
    public Collider body;

    public float middleBlockHeight;
    public float topHeight;
    public float middleHeight;
    public float bottomHeight;
    private float currentHeight;

    public Transform playerTeleportPoint;

    public Phase phase;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        currentHeight = blocks[0].transform.position.y;
        phase = Phase.ORIGIN;
        health = 5;
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        while (health > 0)
        {
            switch (phase)
            {
                case Phase.BLOCKS:
                    yield return StartCoroutine(Blocks());
                    break;
                case Phase.SHOT:
                    yield return StartCoroutine(Shot());
                    break;
                case Phase.STUN:
                    yield return StartCoroutine(Stun());
                    break;
                default:
                    yield return StartCoroutine(Origin());
                    break;
            }
        }
    }


    private IEnumerator Origin()
    {
        foreach (GameObject g in blocks)
        {
            g.transform.position = new Vector3(g.transform.position.x, currentHeight, g.transform.position.z);
        }
        middleBlock.transform.position = new Vector3(middleBlock.transform.position.x, currentHeight, middleBlock.transform.position.z);
        phase = Phase.BLOCKS;
        yield return null;
    }


    private IEnumerator Blocks()
    {
        transform.position = new Vector3(transform.position.x, middleBlockHeight + 50, transform.position.z);
        List<GameObject> temp = new List<GameObject>();
        foreach(GameObject g in blocks)
        {
            temp.Add(g);
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject g = temp[UnityEngine.Random.Range(0, temp.Count)];
            g.transform.position = new Vector3(g.transform.position.x, bottomHeight, g.transform.position.z);
            temp.Remove(g);
        }
        for (int i = 0; i < 2; i++)
        {
            GameObject g = temp[UnityEngine.Random.Range(0, temp.Count)];
            g.transform.position = new Vector3(g.transform.position.x, middleHeight, g.transform.position.z);
            temp.Remove(g);
        }
        temp[0].transform.position = new Vector3(temp[0].transform.position.x, topHeight, temp[0].transform.position.z);
        middleBlock.transform.position = new Vector3(middleBlock.transform.position.x, middleBlockHeight, middleBlock.transform.position.z);
        yield return new WaitForSeconds(30f);
        foreach(GameObject g in blocks)
        {
            g.transform.position = new Vector3(g.transform.position.x, currentHeight, g.transform.position.z);
        }
        middleBlock.transform.position = new Vector3(middleBlock.transform.position.x, currentHeight, middleBlock.transform.position.z);
        yield return new WaitForSeconds(1f);
        phase = Phase.SHOT;
        yield return null;
    }

    private IEnumerator Stun()
    {
        yield return new WaitForSeconds(10f);
        phase = Phase.BLOCKS;
        yield return null;
    }

    private IEnumerator Shot()
    {
        body.enabled = true;
        top.enabled = false;
        int shots = 15;
        for (int i = 0; i < shots; i++)
        {
            transform.Rotate(new Vector3(0f, 360f / shots, 0f));
            Instantiate(shot, transform.position + (transform.forward * 5) + (transform.up * 10), new Quaternion());
            yield return new WaitForSeconds(0.5f);
        }
        phase = Phase.STUN;
        yield return null;
    }
}

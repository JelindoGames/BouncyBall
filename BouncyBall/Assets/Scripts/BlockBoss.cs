using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockBoss : MonoBehaviour
{
    public enum Phase
    {
        ORIGIN, BLOCKS, SHOT, STUN
    }

    public List<GameObject> blocks;
    public GameObject middleBlock;
    public GameObject shot;

    public float middleBlockHeight;
    public float topHeight;
    public float middleHeight;
    public float bottomHeight;
    private float currentHeight;

    public Transform playerTeleportPoint;
    private Transform player;

    public Phase phase;
    private int health;

    public GameObject healthImg;
    public StoryTalkInstance story;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHeight = blocks[0].transform.position.y;
        phase = Phase.ORIGIN;
        health = 5;
        StartCoroutine(Sequence());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CompleteReset();
        }
    }

    IEnumerator Sequence()
    {
        while (health > 0)
        {
            if (!story.inStory)
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
    }


    private IEnumerator Origin()
    {
        foreach (GameObject g in blocks)
        {
            g.transform.position = new Vector3(g.transform.position.x, currentHeight, g.transform.position.z);
        }
        middleBlock.transform.position = new Vector3(middleBlock.transform.position.x, currentHeight, middleBlock.transform.position.z);
        yield return new WaitForSeconds(0.2f);
        if (health % 2 == 1)
            phase = Phase.SHOT;
        else
            phase = Phase.BLOCKS;
        yield return null;
    }


    private IEnumerator Blocks()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.position, player.up * -1, out hit))
        {
            if (hit.transform.gameObject.tag == "Special Block")
            {
                CallHit();
            }
        }

        transform.position = new Vector3(transform.position.x, middleBlockHeight + 50, transform.position.z);
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject g in blocks)
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
        foreach (GameObject g in blocks)
        {
            g.transform.position = new Vector3(g.transform.position.x, currentHeight, g.transform.position.z);
        }
        middleBlock.transform.position = new Vector3(middleBlock.transform.position.x, currentHeight, middleBlock.transform.position.z);
        yield return new WaitForSeconds(1.5f);
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
        int shots = 15;
        for (int i = 0; i < shots; i++)
        {
            transform.Rotate(new Vector3(0f, 360f / shots, 0f));
            GameObject g = Instantiate(shot, transform.position + (transform.forward * 5) + (transform.up * 10.5f), new Quaternion());
            g.transform.Rotate(0, (360f / shots) * i, 0f);
            yield return new WaitForSeconds(0.2f);
        }
        phase = Phase.STUN;
        yield return null;
    }

    public void CallHit()
    {
        FindObjectOfType<ColorChanger>().beingTransported = true;
        StartCoroutine("Hit");
    }

    public IEnumerator Hit()
    {
        health--;
        healthImg.GetComponent<Image>().fillAmount = (float)health / 5;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        yield return StartCoroutine("MoveToOrigin");
        //player.GetComponent<Movement>().DropParticles(false);

        Reset();
        yield return null;
    }

    private IEnumerator MoveToOrigin()
    {
        while (Vector3.Distance(playerTeleportPoint.position, player.position) >= 0.5f)
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            player.position = Vector3.Lerp(player.position, playerTeleportPoint.position, 0.1f);
            yield return new WaitForEndOfFrame();
        }
        FindObjectOfType<ColorChanger>().beingTransported = false;
        yield return null;
    }

    private void Reset()
    {
        healthImg.GetComponent<Image>().fillAmount = (float)health / 5;
        StopAllCoroutines();
        phase = Phase.ORIGIN;
        StartCoroutine("Sequence");
    }

    public void CompleteReset()
    {
        currentHeight = blocks[0].transform.position.y;
        health = 5;
        Reset();
    }

    private void OnDestroy()
    {
        Debug.Log("You won!");
    }
}

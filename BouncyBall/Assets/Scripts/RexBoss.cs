using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class RexBoss : MonoBehaviour
{
    enum Phase
    {
        ORIGIN, LIGHTS, FEET, HANDS, COMBO, STUN
    }

    public float clipVolume = .6f;

    public Transform playerTeleportPoint;

    public GameObject storyEnd;
    public GameObject healthBackgroundImage;
    public GameObject healthImg;

    public Vector3[] lightPosList = new Vector3[3];

    public Transform[] shoeTranList = new Transform[4];

    public Transform[] handTranList = new Transform[4];

    public Vector3[] coinList = new Vector3[4];

    public GameObject stunPlatform;
    public GameObject light1;
    public GameObject light2;
    public GameObject shoe1;
    public GameObject shoe2;
    public GameObject hand1;
    public GameObject hand2;

    public GameObject spiritGun;
    public GameObject goldenGun;
    public GameObject coin;

    public AudioClip hitClip;

    public ButtonBehaviour buttXD;

    int health = 7;
    Phase phase;
    Transform player;

    public float timer = 0;
    public int comboCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        phase = Phase.ORIGIN;
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        while (health > 0)
        {
            if (LevelManager.levelPlaying)
            {
                switch (phase)
                {
                    case Phase.LIGHTS:
                        yield return StartCoroutine(Lights());
                        break;
                    case Phase.FEET:
                        yield return StartCoroutine(Feet());
                        break;
                    case Phase.HANDS:
                        yield return StartCoroutine(Hands());
                        break;
                    case Phase.COMBO:
                        yield return StartCoroutine(Combo());
                        break;
                    case Phase.STUN:
                        yield return StartCoroutine(Stun());
                        break;
                    default:
                        yield return StartCoroutine(Origin());
                        break;
                }
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
        yield return null;
    }

    private IEnumerator Stun()
    {
        if (health == 6 || health == 3 || health == 1)
        {
            stunPlatform.SetActive(true);
        }
        yield return new WaitForSeconds(7);

        StartCoroutine(Origin());
        yield return null;
    }

    private IEnumerator Combo()
    {
        if (hand1.activeSelf == false)
        {
            int i = new Random().Next(0, 4);
            hand1.transform.position = handTranList[i].position;
            hand1.transform.rotation = handTranList[i].rotation;
            hand1.SetActive(true);
        }

        if (shoe1.activeSelf == false)
        {
            int i = new Random().Next(0, 4);
            shoe1.transform.position = shoeTranList[i].position;
            shoe1.transform.rotation = shoeTranList[i].rotation;
            shoe1.SetActive(true);
        }

        if (light1.activeSelf == false)
        {
            light1.transform.position = lightPosList[0];
            light1.SetActive(true);
        }

        if (timer > comboCount * 10)
        {
            hand1.SetActive(false);
            hand2.SetActive(false);
            shoe1.SetActive(false);
            shoe2.SetActive(false);
            comboCount++;
        }

        if (comboCount >= 4)
        {
            phase = Phase.STUN;
        }

        timer += Time.deltaTime;

        yield return null;
    }

    private IEnumerator Hands()
    {
        switch (health)
        {
            case 5:
                {
                    if (hand1.activeSelf == false)
                    {
                        int i = new Random().Next(0, 4);
                        hand1.transform.position = handTranList[i].position;
                        hand1.transform.rotation = handTranList[i].rotation;
                        hand1.SetActive(true);
                        if (GameObject.FindGameObjectsWithTag("Coin").Length <= 0)
                        {
                            foreach (Vector3 vec in coinList)
                            {
                                Instantiate(coin, vec, coin.transform.rotation);
                            }
                        }
                    }
                    break;
                }
            case 2:
                {
                    if (hand1.activeSelf == false || hand2.activeSelf == false)
                    {
                        int i = new Random().Next(0, 4);
                        int k = i;
                        while (k == i)
                        {
                            k = new Random().Next(0, 4);
                        }
                        if (GameObject.FindGameObjectsWithTag("Coin").Length <= 0)
                        {
                            foreach (Vector3 vec in coinList)
                            {
                                Instantiate(coin, vec, coin.transform.rotation);
                            }
                        }
                        hand1.transform.position = handTranList[i].position;
                        hand1.transform.rotation = handTranList[i].rotation;
                        hand1.SetActive(true);
                        yield return new WaitForSeconds(1);
                        hand2.transform.position = handTranList[k].position;
                        hand2.transform.rotation = handTranList[k].rotation;
                        hand2.SetActive(true);
                    }
                    break;
                }
        }


        if (timer > 10)
        {
            timer = 0;
            hand1.SetActive(false);
            hand2.SetActive(false);
        }

        timer += Time.deltaTime;

        if (GameObject.FindGameObjectsWithTag("Coin").Length <= 0)
        {
            StartShot();
            phase = Phase.STUN;
        }

        yield return null;
    }

    private IEnumerator Feet()
    {
        switch (health)
        {
            case 6:
                {
                    if (shoe1.activeSelf == false)
                    {
                        int i = new Random().Next(0, 4);
                        shoe1.transform.position = shoeTranList[i].position;
                        shoe1.transform.rotation = shoeTranList[i].rotation;
                        shoe1.SetActive(true);
                    }
                    break;
                }
            case 3:
                {
                    if (shoe1.activeSelf == false || shoe2.activeSelf == false)
                    {
                        int i = new Random().Next(0, 4);
                        int k = i;
                        while (k == i)
                        {
                            k = new Random().Next(0, 4);
                        }
                        shoe1.transform.position = shoeTranList[i].position;
                        shoe1.transform.rotation = shoeTranList[i].rotation;
                        shoe1.SetActive(true);
                        shoe2.transform.position = shoeTranList[k].position;
                        shoe2.transform.rotation = shoeTranList[k].rotation;
                        shoe2.SetActive(true);
                    }
                    break;
                }
        }
        yield return new WaitForSeconds(9f);

        phase = Phase.STUN;

        yield return null;
    }

    private IEnumerator Lights()
    {
        switch (health)
        {
            case 7:
                {
                    if (light1.activeSelf == false)
                    {
                        light1.transform.position = lightPosList[0];
                        light1.SetActive(true);
                        foreach (Vector3 vec in coinList)
                        {
                            Instantiate(coin, vec, coin.transform.rotation);
                        }
                    }
                    break;
                }
            case 4:
                {
                    if (light1.activeSelf == false || light2.activeSelf == false)
                    {
                        light1.transform.position = lightPosList[1];
                        light1.SetActive(true);
                        light2.transform.position = lightPosList[2];
                        light2.SetActive(true);
                        foreach (Vector3 vec in coinList)
                        {
                            Instantiate(coin, vec, coin.transform.rotation);
                        }
                    }
                    break;
                }
        }

        if (GameObject.FindGameObjectsWithTag("Coin").Length <= 0)
        {
            StartShot();
            phase = Phase.STUN;
        }

        yield return null;
    }

    private IEnumerator Origin()
    {
        timer = 0;
        comboCount = 1;
        stunPlatform.SetActive(false);
        buttXD.isPressed = false;
        light1.SetActive(false);
        light2.SetActive(false);
        shoe1.SetActive(false);
        shoe2.SetActive(false);
        hand1.SetActive(false);
        hand2.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        switch (health)
        {
            case 7:
                phase = Phase.LIGHTS;
                break;
            case 6:
                phase = Phase.FEET;
                break;
            case 5:
                phase = Phase.HANDS;
                break;
            case 4:
                phase = Phase.LIGHTS;
                break;
            case 3:
                phase = Phase.FEET;
                break;
            case 2:
                phase = Phase.HANDS;
                break;
            case 1:
                phase = Phase.COMBO;
                break;
            default:
                break;
        }

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            CallHit();
            SpiritShot ss = other.GetComponent<SpiritShot>();
            ss.explosion.enabled = true;
            ss.shot.enabled = false;
        }
    }

    public void CallHit()
    {
        FindObjectOfType<ColorChanger>().beingTransported = true;
        StartCoroutine("Hit");
    }

    public IEnumerator Hit()
    {
        health--;
        healthImg.GetComponent<Image>().fillAmount = (float)health / 7;
        if (health <= 0)
        {
            storyEnd.SetActive(true);
            Destroy(healthImg);
            Destroy(healthBackgroundImage);
        }
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        timer = 0;
        comboCount = 1;
        stunPlatform.SetActive(false);
        buttXD.isPressed = false;
        light1.SetActive(false);
        light2.SetActive(false);
        shoe1.SetActive(false);
        shoe2.SetActive(false);
        hand1.SetActive(false);
        hand2.SetActive(false);

        FindObjectOfType<LevelManager>().Play2DAudio(hitClip, clipVolume);

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
        healthImg.GetComponent<Image>().fillAmount = (float)health / 7;

        GameObject[] list = GameObject.FindGameObjectsWithTag("Coin");

        foreach (GameObject g in list)
        {
            Destroy(g);
        }

        StopAllCoroutines();
        phase = Phase.ORIGIN;
        StartCoroutine("Sequence");
    }

    public void StartShot()
    {
        if (health > 1)
            Instantiate(spiritGun, new Vector3(28, 0, 0), new Quaternion());
        else
            Instantiate(goldenGun, new Vector3(28, 0, 0), new Quaternion());
    }
}

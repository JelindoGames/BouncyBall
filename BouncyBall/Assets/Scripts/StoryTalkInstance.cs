using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTalkInstance : MonoBehaviour
{
    public List<string> storyText;
    public List<string> nameText;
    public Sprite storyImage;
    public GameObject destroyObject;

    StoryTalk m;
    int textNum = 0;
    //AudioSource aS;
    //AudioSource playerAudio;
    public bool inStory;

    // Start is called before the first frame update
    void Start()
    {
        m = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StoryTalk>();
        m.SetImage(storyImage);
        m.StoryStart();
        inStory = true;
        /*aS = GetComponent<AudioSource>();
        playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        playerAudio.Pause();
        aS.Play();*/
        if (destroyObject == null)
            destroyObject = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Pressed");
            textNum++;
        }
        try
        {
            m.StoryStart();
            m.SetText(storyText[textNum], nameText[textNum]);
            m.EnableCanvas(true);
        }
        catch (Exception e)
        {
            Debug.Log("Object Destroyed");
            m.EnableCanvas(false);
            m.StoryEnd();
            /*aS.Stop();
            playerAudio.Play();*/
            inStory = false;
            Destroy(destroyObject);
        }
    }
}

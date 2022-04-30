using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTalkInstance : MonoBehaviour
{
    // Vars
    public List<string> storyText;
    public List<string> nameText;
    public Sprite storyImage;
    public GameObject destroyObject;

    StoryTalk m;
    int textNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        m = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StoryTalk>();
        m.SetImage(storyImage);
        m.StoryStart();
        if (destroyObject == null)
            destroyObject = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Advances Text
            Debug.Log("Pressed");
            textNum++;
        }
        try
        {
            // Sets the fields of StoryTalk
            m.StoryStart();
            m.SetText(storyText[textNum], nameText[textNum]);
            m.EnableCanvas(true);
        }
        catch (Exception e)
        {
            // If the story is over do this
            Debug.Log("Object Destroyed");
            m.EnableCanvas(false);
            m.StoryEnd();
            Destroy(destroyObject);
        }
    }

    // To enable from Event
    public void EnableThis()
    {
        this.enabled = true;
    }
}

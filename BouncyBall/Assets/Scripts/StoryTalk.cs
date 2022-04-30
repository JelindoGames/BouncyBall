using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class StoryTalk : MonoBehaviour
{
    // Variables 
    public VolumeProfile normal;
    public VolumeProfile talkVol;
    public GameObject storyPanel;
    public Text storyText;
    public Text nameText;
    public Image storyImage;

    public Volume gV;
    // Start is called before the first frame update
    void Start()
    {
        gV.profile = normal;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Start story
    public void StoryStart()
    {
        gV.profile = talkVol;
        LevelManager.levelPlaying = false;
    }

    // End story
    public void StoryEnd()
    {
        gV.profile = normal;
        LevelManager.levelPlaying = true;
    }

    // Sets the image on the canvas
    public void SetImage(Sprite img)
    {
        storyImage.sprite = img;
        storyImage.GetComponent<RectTransform>().sizeDelta = new Vector2(img.rect.width / 2, img.rect.height / 2);
    }

    // Sets the text in the panel
    public void SetText(string str, string name)
    {
        nameText.text = name + ":";
        storyText.text = str;
    }

    // Enables from Event
    public void EnableCanvas(bool x)
    {
        storyPanel.SetActive(x);
    }
}

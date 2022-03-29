using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class StoryTalk : MonoBehaviour
{
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

    public void StoryStart()
    {
        gV.profile = talkVol;
    }

    public void StoryEnd()
    {
        gV.profile = normal;
        LevelManager.levelPlaying = true;
    }

    public void SetImage(Sprite img)
    {
        /*storyImage.sprite = img;
        storyImage.GetComponent<RectTransform>().sizeDelta = new Vector2(img.rect.width / 2, img.rect.height / 2);*/
    }

    public void SetText(string str, string name)
    {
        nameText.text = name + ":";
        storyText.text = str;
    }

    public void EnableCanvas(bool x)
    {
        storyPanel.SetActive(x);
    }
}

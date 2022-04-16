using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Declares that it is the current level when it starts.
public class LevelDeclarator : MonoBehaviour
{
    [SerializeField] int world;
    [SerializeField] bool declareOnAwake;
    [SerializeField] float declareSpeed;
    [SerializeField] Image myImg;
    [SerializeField] Text myText;
    int curLevel = 1;
    [SerializeField] string bossName = "";

    private void Start()
    {
        if (declareOnAwake)
        {
            DeclareLevel(false);
        }
        else
        {
            myImg.color = new Color(0, 0, 0, 0);
            myText.color = new Color(0, 0, 0, 0);
        }
    }

    public void AdvanceLevel()
    {
        curLevel++;
    }

    public void DeclareLevel(bool newLevel)
    {
        StartCoroutine(DeclareProcess(newLevel));
    }

    IEnumerator DeclareProcess(bool newLevel)
    {
        curLevel += newLevel ? 1 : 0;
        if (bossName == "")
            myText.text = "Level " + world + " - " + curLevel;
        else
            myText.text = bossName;
        // Opaque
        myImg.color += Color.black;
        myText.color += Color.black;

        while (myText.color.a > 0)
        {
            yield return new WaitForEndOfFrame();
            myImg.color -= Color.black * declareSpeed * Time.deltaTime;
            myText.color -= Color.black * declareSpeed * Time.deltaTime;
        }
    }
}

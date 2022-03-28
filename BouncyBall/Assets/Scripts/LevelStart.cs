using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    LevelManager level;

    public int levelNum;
    // Start is called before the first frame update
    void Start()
    {
        level = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        level.currentLevelIdx = levelNum;
        LevelManager.levelPlaying = true;
    }
}

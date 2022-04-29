using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStart : MonoBehaviour
{
    LevelManager level;
    public int levelNum;
    [SerializeField] GameObject audioPlayer;
    [SerializeField] AudioClip newLevelSound;
    [SerializeField] GameObject newLevelParticles;
    [SerializeField] bool endOfWorld;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        level = FindObjectOfType<LevelManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void StartLevel()
    {
        if (endOfWorld)
        {
            level.PlayerHitsWorldEnd();
            return;
        }
        level.PlayerHitsSubworldEnd(levelNum);
        //level.currentLevelIdx = levelNum;
        //LevelManager.levelPlaying = true;
        AudioSource audio = Instantiate(audioPlayer, player.transform.position, Quaternion.identity).GetComponent<AudioSource>();
        audio.clip = newLevelSound;
        audio.Play();
        Instantiate(newLevelParticles, player.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

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
    [SerializeField] bool endOfWorld; // When StartLevel() is called, will that signify the world is done?
    GameObject player;

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
        AudioSource audio = Instantiate(audioPlayer, player.transform.position, Quaternion.identity).GetComponent<AudioSource>();
        audio.clip = newLevelSound;
        audio.Play();
        Instantiate(newLevelParticles, player.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

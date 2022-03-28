using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    LevelManager level;
    public int levelNum;
    [SerializeField] GameObject audioPlayer;
    [SerializeField] AudioClip sound;
    [SerializeField] GameObject particles;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        level = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void StartLevel()
    {
        level.currentLevelIdx = levelNum;
        LevelManager.levelPlaying = true;
        AudioSource audio = Instantiate(audioPlayer, player.transform.position, Quaternion.identity).GetComponent<AudioSource>();
        audio.clip = sound;
        audio.Play();
        Instantiate(particles, player.transform.position, Quaternion.identity);
        FindObjectOfType<LevelDeclarator>().DeclareLevel(true);
        Destroy(gameObject);
    }
}

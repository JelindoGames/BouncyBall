using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStart : MonoBehaviour
{
    LevelManager level;
    public int levelNum;
    [SerializeField] GameObject audioPlayer;
    [SerializeField] AudioClip sound;
    [SerializeField] GameObject particles;
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
        print(endOfWorld);
        if (endOfWorld)
        {
            level.PlayerHitsLevelEnd();
            return;
        }
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

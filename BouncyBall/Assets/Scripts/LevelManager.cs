using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Handles all of the major level-related states in the game.
public class LevelManager : MonoBehaviour
{
    GameObject player;
    StoryTalk st;

    public static bool levelPlaying;
    bool levelWon;

    [SerializeField] int world; // World idx? (0 = World 1, 1 = World 1 boss...)

    [SerializeField] GameObject winText;
    [SerializeField] GameObject deathText;
    [SerializeField] Text timeText;
    [SerializeField] Text coinText;
    [SerializeField] Text levelText;

    [SerializeField] AudioClip levelReset;
    [SerializeField] AudioClip coinCollected;
    [SerializeField] AudioClip deathAudio;
    [SerializeField] GameObject audioPlayer;

    [SerializeField] Transform[] levelStarts;

    public static Transform currentSpawn;

    public int currentLevelIdx;
    public float alteringSpeed = 0.2f;

    float currentTime;
    int coinScore = 0;

    private void Awake()
    {
        levelWon = false;
        levelPlaying = true;
        winText.SetActive(false);
        deathText.SetActive(false);
    }

    private void Start()
    {
        // Update these for beginning of the world
        PlayerPrefs.SetInt("world", world);
        currentTime = PlayerPrefs.GetFloat("time", 0);
        coinScore = PlayerPrefs.GetInt("score", 0);

        player = GameObject.FindGameObjectWithTag("Player");
        st = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StoryTalk>();

        player.transform.position = levelStarts[currentLevelIdx].position;
        currentSpawn = levelStarts[currentLevelIdx];

        st.EnableCanvas(false);

        UpdateCoinText();
        UpdateLevelText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !levelWon)
        {
            PlayerReset();
        }
        currentTime += Time.deltaTime;
        timeText.text = "Time: " + currentTime.ToString("0.00");
    }

    public void PlayerHitsSubworldEnd(int levelNum)
    {
        currentLevelIdx = levelNum;
        currentSpawn = levelStarts[currentLevelIdx];
        FindObjectOfType<LevelDeclarator>().DeclareLevel(true);
        UpdateLevelText();
    }

    public void PlayerHitsWorldEnd()
    {
        StartCoroutine(VictorySequence(false, null, null));
    }

    // For story stuff
    public void PlayerHitsWorldEnd(GameObject collided, GameObject character)
    {
        StartCoroutine(VictorySequence(true, collided, character));
    }

    public void PlayerReset()
    {
        Play2DAudio(levelReset);
        player.transform.position = levelStarts[currentLevelIdx].position;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }


    public IEnumerator PlayerHitsDeathPlane()
    {
        deathText.SetActive(true);
        Play2DAudio(deathAudio);
        levelPlaying = false;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));
        FindObjectOfType<LevelDeclarator>().DeclareLevel(false);
        deathText.SetActive(false);
        levelPlaying = true;

    }

    IEnumerator VictorySequence(bool story, GameObject collided, GameObject character)
    {
        levelWon = true;
        levelPlaying = false;
        winText.SetActive(true);
        FindObjectOfType<LevelDeclarator>().AdvanceLevel();

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        PlayerPrefs.SetFloat("time", currentTime);
        PlayerPrefs.SetInt("score", coinScore);
        PlayerPrefs.Save();

        if (!story)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        winText.SetActive(false);

        collided.GetComponent<StoryTalkInstance>().enabled = true;
        currentLevelIdx = collided.transform.parent.GetComponent<LevelStart>().levelNum - 1;

        player.transform.position = levelStarts[currentLevelIdx].position;

        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        yield return null;
    }

    public void CoinCollected()
    {
        coinScore++;
        UpdateCoinText();
        Play2DAudio(coinCollected);
    }

    void UpdateCoinText()
    {
        coinText.text = "Score: " + coinScore;
    }

    void UpdateLevelText()
    {
        levelText.text = "World " + (world + 1) + "-" + (currentLevelIdx + 1);
    }


    public void Play2DAudio(AudioClip clip)
    {
        AudioSource audio = Instantiate(audioPlayer, Camera.main.transform.position, Quaternion.identity).GetComponent<AudioSource>();
        audio.clip = clip;
        audio.Play();
    }

    public void Play2DAudio(AudioClip clip, float volume)
    {
        AudioSource audio = Instantiate(audioPlayer, Camera.main.transform.position, Quaternion.identity).GetComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = volume;
        audio.Play();
    }
}
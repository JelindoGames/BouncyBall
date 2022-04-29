using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    GameObject player;
    StoryTalk st;

    public static bool levelPlaying;
    bool levelWon;
    [SerializeField] int world; // World idx? (0 = World 1, 1 = World 1 boss...)
    [SerializeField] GameObject winText;
    [SerializeField] AudioClip deathAudio;
    [SerializeField] GameObject deathText;
    [SerializeField] Text timeText;
    [SerializeField] Text coinText;
    [SerializeField] GameObject audioPlayer;

    [SerializeField] Transform[] levelStarts;

    [SerializeField] AudioClip levelReset;
    [SerializeField] AudioClip coinCollected;

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
        PlayerPrefs.SetInt("world", world);
        //currentLevelIdx = PlayerPrefs.GetInt("currentSpawn", 0);
        currentTime = PlayerPrefs.GetFloat("time", 0);

        player = GameObject.FindGameObjectWithTag("Player");
        st = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StoryTalk>();

        player.transform.position = levelStarts[currentLevelIdx].position;
        currentSpawn = levelStarts[currentLevelIdx];

        st.EnableCanvas(false);

        UpdateCoinText();
    }

    private void Update()
    {
        currentSpawn = levelStarts[currentLevelIdx];
        if (Input.GetKeyDown(KeyCode.R) && !levelWon)
        {
            PlayerReset();
        }
        currentTime += Time.deltaTime;
        PlayerPrefs.SetFloat("time", currentTime);
        //PlayerPrefs.GetInt("currentSpawn", currentLevelIdx);
        PlayerPrefs.Save();
        timeText.text = "Time: " + currentTime;
    }

    public void PlayerHitsLevelEnd()
    {
        StartCoroutine(VictorySequence(false, null, null));
    }

    // For story stuff
    public void PlayerHitsLevelEnd(GameObject collided, GameObject character)
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
        winText.SetActive(true);
        levelPlaying = false;
        FindObjectOfType<LevelDeclarator>().AdvanceLevel();
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
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

    void Play2DAudio(AudioClip clip)
    {
        AudioSource audio = Instantiate(audioPlayer, Camera.main.transform.position, Quaternion.identity).GetComponent<AudioSource>();
        audio.clip = clip;
        audio.Play();
    }
}
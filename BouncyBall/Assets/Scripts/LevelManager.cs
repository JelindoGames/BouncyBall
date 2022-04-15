using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    GameObject player;
    //CameraMove cam;
    StoryTalk st;


    public static bool levelPlaying;
    [SerializeField] GameObject winText;
    [SerializeField] AudioClip deathAudio;
    [SerializeField] GameObject deathText;
    [SerializeField] Text timeText;
    [SerializeField] Text coinText;
    [SerializeField] Vector3 cameraStartPos;
    [SerializeField] Transform cam;
    [SerializeField] GameObject audioPlayer;

    [SerializeField] Transform[] levelStarts;
    [SerializeField] List<FakeTransform> cameraPos;

    [SerializeField] AudioClip levelReset;
    [SerializeField] AudioClip coinCollected;

    public static Transform currentSpawn;
    public static FakeTransform currentCamPos;

    public int currentLevelIdx;
    public float alteringSpeed = 0.2f;

    float currentTime;
    int coinScore = 0;

    private void Awake()
    {
        levelPlaying = true;
        winText.SetActive(false);
        deathText.SetActive(false);
    }

    private void Start()
    {
        cam.position = cameraStartPos;
        currentLevelIdx = PlayerPrefs.GetInt("currentSpawn", 0);
        currentTime = PlayerPrefs.GetFloat("time", 0);

        player = GameObject.FindGameObjectWithTag("Player");
        st = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StoryTalk>();

        player.transform.position = levelStarts[currentLevelIdx].position;
        currentSpawn = levelStarts[currentLevelIdx];
        currentCamPos = cameraPos[currentLevelIdx];

        st.EnableCanvas(false);

        UpdateCoinText();
    }

    private void Update()
    {
        currentSpawn = levelStarts[currentLevelIdx];
        currentCamPos = cameraPos[currentLevelIdx];
        if (Input.GetKeyDown(KeyCode.R))
        {
            Play2DAudio(levelReset);
            cam.position = cameraStartPos;
            player.transform.position = levelStarts[currentLevelIdx].position;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            //player.GetComponent<Movement>().DropParticles(false);
        }
        currentTime += Time.deltaTime;
        PlayerPrefs.SetFloat("time", currentTime);
        PlayerPrefs.GetInt("currentSpawn", currentLevelIdx);
        PlayerPrefs.Save();
        timeText.text = "Time: " + currentTime;
    }

    public void PlayerHitsLevelEnd()
    {
        StartCoroutine(VictorySequence(false, null, null)); // TODO this code is bad
    }

    // For story stuff
    public void PlayerHitsLevelEnd(GameObject collided, GameObject character)
    {
        StartCoroutine(VictorySequence(true, collided, character));
    }

    public IEnumerator PlayerHitsDeathPlane()
    {
        deathText.SetActive(true);
        Play2DAudio(deathAudio);
        levelPlaying = false;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));
        cam.position = cameraStartPos;
        FindObjectOfType<LevelDeclarator>().DeclareLevel(false);
        deathText.SetActive(false);
        levelPlaying = true;
    }

    IEnumerator VictorySequence(bool story, GameObject collided, GameObject character)
    {
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

        //SceneManager.LoadScene(nextLevelID);

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

[System.Serializable]
public struct FakeTransform
{
    public Vector3 offsetPos;
    public Quaternion offsetRot;
    public Vector3 offsetSca;
}
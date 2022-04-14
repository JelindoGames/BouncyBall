using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] Vector3 cameraStartPos;
    [SerializeField] Transform cam;
    [SerializeField] GameObject audioPlayer;

    [SerializeField] List<Transform> levelStarts;
    [SerializeField] List<FakeTransform> cameraPos;

    [SerializeField] AudioClip levelReset;

    public static Transform currentSpawn;
    public static FakeTransform currentCamPos;

    public int currentLevelIdx;
    public float alteringSpeed = 0.2f;

    float currentTime;

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
        //cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();
        st = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StoryTalk>();

        player.transform.position = levelStarts[currentLevelIdx].position;
        /*
        cam.offsetPos = cameraPos[currentLevelIdx].offsetPos;
        cam.offsetRot = cameraPos[currentLevelIdx].offsetRot;
        cam.offsetSca = cameraPos[currentLevelIdx].offsetSca;
        */
        currentSpawn = levelStarts[currentLevelIdx];
        currentCamPos = cameraPos[currentLevelIdx];

        st.EnableCanvas(false);
    }

    public void UpdateCam()
    {
        /*
        if (cam.offsetPos != cameraPos[currentLevelIdx].offsetPos)
        {
            cam.offsetPos += (cameraPos[currentLevelIdx].offsetPos - cam.offsetPos) * alteringSpeed;
        }
        if (cam.offsetRot != cameraPos[currentLevelIdx].offsetRot)
        {
            cam.offsetRot = Quaternion.Slerp(cam.offsetRot, cameraPos[currentLevelIdx].offsetRot, alteringSpeed);
        }
        cam.offsetSca = cameraPos[currentLevelIdx].offsetSca;
        */
    }

    private void Update()
    {
        currentSpawn = levelStarts[currentLevelIdx];
        currentCamPos = cameraPos[currentLevelIdx];
        UpdateCam();
        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioSource audio = Instantiate(audioPlayer, Camera.main.transform.position, Quaternion.identity).GetComponent<AudioSource>();
            audio.clip = levelReset;
            audio.Play();
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

    public void PlayerHitsLevelEnd(GameObject collided, GameObject character)
    {
        StartCoroutine(VictorySequence(collided, character));
    }

    public IEnumerator PlayerHitsDeathPlane()
    {
        deathText.SetActive(true);
        AudioSource.PlayClipAtPoint(deathAudio, Camera.main.transform.position);
        levelPlaying = false;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));
        /*
        AudioSource audio = Instantiate(audioPlayer, Camera.main.transform.position, Quaternion.identity).GetComponent<AudioSource>();
        audio.clip = levelReset;
        audio.Play();
        */
        cam.position = cameraStartPos;
        FindObjectOfType<LevelDeclarator>().DeclareLevel(false);
        deathText.SetActive(false);
        levelPlaying = true;
    }

    IEnumerator VictorySequence(GameObject collided, GameObject character)
    {
        winText.SetActive(true);
        levelPlaying = false;
        FindObjectOfType<LevelDeclarator>().AdvanceLevel();
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        winText.SetActive(false);

        collided.GetComponent<StoryTalkInstance>().enabled = true;
        currentLevelIdx = collided.transform.parent.GetComponent<LevelStart>().levelNum - 1;

        player.transform.position = levelStarts[currentLevelIdx].position;

        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        yield return null;

        //SceneManager.LoadScene(nextLevelID);

    }
}

[System.Serializable]
public struct FakeTransform
{
    public Vector3 offsetPos;
    public Quaternion offsetRot;
    public Vector3 offsetSca;
}
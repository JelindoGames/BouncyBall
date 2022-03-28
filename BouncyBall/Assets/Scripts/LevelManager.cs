using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    GameObject player;
    CameraMove cam;
    StoryTalk st;


    [SerializeField] int nextLevelID;
    public static bool levelPlaying;
    [SerializeField] GameObject winText;
    [SerializeField] GameObject deathText;
    [SerializeField] Text timeText;

    [SerializeField] List<Transform> levelStarts;
    [SerializeField] List<FakeTransform> cameraPos;

    public static Transform currentSpawn;
    public static FakeTransform currentCamPos;

    public static int currentLevelIdx;
    public float alteringSpeed = 0.2f;


    private void Awake()
    {
        currentLevelIdx = 0;
        levelPlaying = true;
        winText.SetActive(false);
        deathText.SetActive(false);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();
        st = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StoryTalk>();

        player.transform.position = levelStarts[currentLevelIdx].position;
        cam.offsetPos = cameraPos[currentLevelIdx].offsetPos;
        cam.offsetRot = cameraPos[currentLevelIdx].offsetRot;
        cam.offsetSca = cameraPos[currentLevelIdx].offsetSca;
        currentSpawn = levelStarts[currentLevelIdx];
        currentCamPos = cameraPos[currentLevelIdx];

        st.EnableCanvas(false);
    }

    public void UpdateCam()
    {
        if (cam.offsetPos != cameraPos[currentLevelIdx].offsetPos)
        {
            cam.offsetPos += (cameraPos[currentLevelIdx].offsetPos - cam.offsetPos) * alteringSpeed;
        }
        if (cam.offsetRot != cameraPos[currentLevelIdx].offsetRot)
        {
            cam.offsetRot = Quaternion.Slerp(cam.offsetRot, cameraPos[currentLevelIdx].offsetRot, alteringSpeed);
        }
        cam.offsetSca = cameraPos[currentLevelIdx].offsetSca;
    }

    private void Update()
    {
        currentSpawn = levelStarts[currentLevelIdx];
        currentCamPos = cameraPos[currentLevelIdx];
        UpdateCam();
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.transform.position = levelStarts[currentLevelIdx].position;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    public void PlayerHitsLevelEnd(GameObject collided, GameObject character)
    {
        StartCoroutine(VictorySequence(collided, character));
    }

    public IEnumerator PlayerHitsDeathPlane()
    {
        deathText.SetActive(true);
        levelPlaying = false;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));
        deathText.SetActive(false);
        levelPlaying = true;
    }

    IEnumerator VictorySequence(GameObject collided, GameObject character)
    {
        winText.SetActive(true);
        levelPlaying = false;
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
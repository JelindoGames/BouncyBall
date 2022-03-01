using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int nextLevelID;
    public static bool levelPlaying;
    [SerializeField] GameObject winText;
    [SerializeField] GameObject deathText;

    private void Awake()
    {
        levelPlaying = true;
        winText.SetActive(false);
        deathText.SetActive(false);
    }

    public void PlayerHitsLevelEnd()
    {
        StartCoroutine("VictorySequence");
    }

    public void PlayerHitsDeathPlane()
    {
        StartCoroutine("DeathSequence");
    }

    IEnumerator VictorySequence()
    {
        print("Victory");
        winText.SetActive(true);
        levelPlaying = false;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        SceneManager.LoadScene(nextLevelID);
    }

    IEnumerator DeathSequence()
    {
        print("Dead");
        deathText.SetActive(true);
        levelPlaying = false;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

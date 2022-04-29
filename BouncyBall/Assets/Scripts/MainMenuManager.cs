using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] int startScene;
    [SerializeField] Text continueText; 

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Either know which world we left off in, or default to zero
        continueText.text = "CONTINUE (WORLD " + (PlayerPrefs.GetInt("world", 0) + 1) + ")";
    }

    public void OnNewGame()
    {
        PlayerPrefs.SetFloat("time", 0);
        SceneManager.LoadScene(startScene);
    }

    public void OnContinue()
    {
        // Skipping title screen and opening cutscene
        SceneManager.LoadScene(PlayerPrefs.GetInt("world", 0) + 2);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}

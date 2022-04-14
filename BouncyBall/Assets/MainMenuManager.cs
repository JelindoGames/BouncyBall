using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] int startScene;

    public void OnPlay()
    {
        SceneManager.LoadScene(startScene);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadWinScreen()
    {
        SceneManager.LoadScene("Win Screen");
    }
    public void LoadNewGame()
    {
        SceneManager.LoadScene("Board");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

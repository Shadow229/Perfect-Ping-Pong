using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(GameManager.Instance.Level);
    }


    public void PlayLevel(int LevelNo)
    {
        if (LevelNo <= GameManager.Instance.MaxLevels && LevelNo <= GameManager.Instance.Level)
        {
            SceneManager.LoadScene(LevelNo);
        }

    }
   public void PlayNextLevel()
    {
        GameManager.Instance.LoadNextScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetGame()
    {
        GameManager.Instance.ResetGame();
    }
 
    

}

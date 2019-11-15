using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;


public class Menu : MonoBehaviour
{

    public GameObject AdManager;

    public void PlayGame()
    {
        SceneManager.LoadScene(GameManager.Instance.Level);
    }


    public void PlayLevel(int LevelNo)
    {
        GameManager.Instance.LoadScene(LevelNo);
    }
   public void PlayNextLevel()
    {
        //play full screen ad before next level
        if (AdManager)
        {
            AdManager.GetComponent<Adverts>().PlayFullScreenAd();
        }

        StartCoroutine(SceneLoadDelay());
    }

    IEnumerator SceneLoadDelay()
    {
        yield return new WaitForSeconds(1f);

        //load next scene
        GameManager.Instance.LoadNextScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMenu()
    {
        AdManager.GetComponent<Adverts>().HideBannerAd();
        SceneManager.LoadScene(0);
    }

    public void ResetGame()
    {
        GameManager.Instance.ResetGame();
    }
}

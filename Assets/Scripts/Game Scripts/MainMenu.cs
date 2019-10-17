﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(GameManager.Instance.Level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadNextScene()
    {
        //increment our unlocked levels
        GameManager.Instance.Level++;
        //load next level (if one exists!)
        if (GameManager.Instance.Level <= GameManager.Instance.MaxLevels)
        {
            SceneManager.LoadScene(GameManager.Instance.Level);
        }
        else
        {
            //go back to the menu
            LoadMenu();
        }
    }

}

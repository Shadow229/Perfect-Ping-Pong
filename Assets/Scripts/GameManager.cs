using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }

    public int Level = 1;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void LoadNextScene()
    {   
        //increment our unlocked levels
        Level++;
        //load next level (if it exists!)
        if (SceneManager.GetSceneByName("Level" + Level.ToString()).IsValid())
        {
            SceneManager.LoadScene(Level);
        }
        else
        {
            //go back to the menu
            SceneManager.LoadScene(0);
        }
    }
}

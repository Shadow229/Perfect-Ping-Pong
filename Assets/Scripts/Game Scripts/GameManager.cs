using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }

    //needed while under dev to stop attempting scene loads not completed yet
    public int MaxLevels = 2;

    //game variables
    public int Level;
    public int ChallengeLevel;
    public int Points;
    public bool LevelReqAchieved = false;
    public bool lastChallenge = false;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadGame()
    {
        //get our persistant level on start
        if (PlayerPrefs.HasKey("Level"))
        {
            Level = PlayerPrefs.GetInt("Level");
        }
        else
        {
            Level = 1;
        }
        //get our persistant level on start
        if (PlayerPrefs.HasKey("ChallengeLevel"))
        {
            ChallengeLevel = PlayerPrefs.GetInt("ChallengeLevel");
        }
        else
        {
            ChallengeLevel = 1;
        }
    }

    public void LevelUnlocked()
    {
        //set it
        PlayerPrefs.SetInt("Level", Level++);
        //save it
        PlayerPrefs.Save();
    }


    public void ChallengeCompleted()
    {
        //set it
        PlayerPrefs.SetInt("ChallengeLevel", ChallengeLevel++);
        //save it
        PlayerPrefs.Save();
    }


    public void LoadNextScene()
    {
        //get current scene level
        string sn = SceneManager.GetActiveScene().name;
        sn = sn.Replace("Level", "");
        int.TryParse(sn, out int CurrLvl);

        //load next level (if one exists!)
        if (CurrLvl < MaxLevels)
        {
            SceneManager.LoadScene(CurrLvl + 1);
        }
        else
        {
            //go back to the menu
            SceneManager.LoadScene(0);
        }
    }

    public void LoadScene(int a_Level)
    {
        //if that level has been unlocked
        if (a_Level <= Level)
        {
            SceneManager.LoadScene(a_Level);
        }
    }


    public void ResetGame()
    {
        //delete our save data
        PlayerPrefs.DeleteAll();
        //reload our variables
        LoadGame();
    }

}

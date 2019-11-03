using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }

    //needed while under dev to stop attempting scene loads not completed yet (could also be used as a lock for paid levels)
    public int MaxLevels = 2;

    //game variables
    [Header("Persistant Variables")]
    public int Level;
    public int ChallengeLevel;
    [Header("Gameplay Variables")]
    public int CurrentChallenge;
    public int CurrentLevel;


   // public int Points;
    public bool ChallengeReqAchieved = false;
    public bool lastChallenge = false;



    private void Start()
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
        //set level
        PlayerPrefs.SetInt("Level", Level++);
        //reset challenge level
        ChallengeLevel = 1;
        PlayerPrefs.SetInt("ChallengeLevel", ChallengeLevel);
        //save it
        PlayerPrefs.Save();
    }


    public void ChallengeCompleted()
    {
        if (lastChallenge)
        {
            LevelUnlocked();
        }
        else
        {
            //set it
            PlayerPrefs.SetInt("ChallengeLevel", ChallengeLevel++);
            //save it
            PlayerPrefs.Save();
        }

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
            CurrentLevel = a_Level;
            SceneManager.LoadScene(a_Level);

            if (a_Level == Level)
            {
                CurrentChallenge = ChallengeLevel;
            }
            else
            {
                CurrentChallenge = 1;
            }
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

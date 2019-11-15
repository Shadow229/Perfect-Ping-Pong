//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }

    //needed while under dev to stop attempting scene loads not completed yet (could also be used as a lock for paid levels)
    public int MaxLevels = 2;

    //game variables
    [Header("Device Info")]
    public float ScreenHeight;
    public float ScreenWidth;
    [Header("Persistant Variables")]
    public int Level;
    public int ChallengeLevel;
    [Header("Gameplay Variables")]
    public int CurrentChallenge;
    public int CurrentLevel;
    [Header("User Settings")]
    public float SensitivityMultiplier;
    public float MusicVol, SFXVol;



    // public int Points;
    public bool ChallengeReqAchieved = false;
    public bool lastChallenge = false;


    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
           // InitialiseAds();
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void LoadGame()
    {
         ScreenWidth = Screen.width;
         ScreenHeight = Screen.height;

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
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            SensitivityMultiplier = PlayerPrefs.GetFloat("Sensitivity");
        }
        else
        {
            SensitivityMultiplier = 1.5f;
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            MusicVol = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            MusicVol = 0.5f;
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SFXVol = PlayerPrefs.GetFloat("SFXVolume");
        }
        else
        {
            SFXVol = 0.5f;
        }

        //set music volume
        GetComponent<AudioSource>().volume = MusicVol;
        //Set button volume
        GameObject.Find("Canvas").GetComponent<AudioSource>().volume = SFXVol;
    }

    public void LevelUnlocked()
    {
        //set level
        PlayerPrefs.SetInt("Level", ++Level);
        //reset challenge level
        ChallengeLevel = 1;
        PlayerPrefs.SetInt("ChallengeLevel", ChallengeLevel);
        //save it
        PlayerPrefs.Save();
    }


    public void ChallengeCompleted()
    {
        //if this was the last challenge, and we're not replaying an old level - unlock the next one
        if (CurrentLevel == Level)
        {
            if (lastChallenge)
            {
                LevelUnlocked();
            }
            else
            {
                //set it
                PlayerPrefs.SetInt("ChallengeLevel", ++ChallengeLevel);
                //save it
                PlayerPrefs.Save();
            }
        }
        

    }


    public void LoadNextScene()
    {
        //load next level (if one exists!)
        if (CurrentLevel < MaxLevels)
        {
            LoadScene(CurrentLevel + 1);
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


    public void SetSensitivity(float s)
    {
        SensitivityMultiplier = s;
        PlayerPrefs.SetFloat("Sensitivity", s);
        //save it
        PlayerPrefs.Save();
    }

    //run this from the gamemanager so its persistant across levels
    //public void PlayBackgroundMusic()
    //{
    //    //get audio
    //    AudioSource aud = GetComponent<AudioSource>();
    //    //play it
    //    aud.Play();
    //}
    //update volume


    public void SetMusicVolume(float vol)
    {
        //save it
        MusicVol = vol;
        PlayerPrefs.SetFloat("MusicVolume", vol);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float vol)
    {
        //save it
        SFXVol = vol;
        PlayerPrefs.SetFloat("SFXVolume", vol);
        PlayerPrefs.Save();
    }


    //private void InitialiseAds()
    //{
    //    string gameID = "";
    //    bool testMode = true;

    //    Advertisement.Initialize(gameID, testMode);
    //}
}

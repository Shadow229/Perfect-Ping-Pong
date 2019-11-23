using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }

    //needed while under dev to stop attempting scene loads not completed yet
    public int MaxLevels = 2;

    //game variables
    //[Header("Device Info")]
    public float ScreenHeight { get; set; }
    public float ScreenWidth { get; set; }
    // [Header("Persistant Variables")]
    public int Level { get; set; }
    public int ChallengeLevel { get; set; }
    //[Header("Gameplay Variables")]
    public int CurrentChallenge { get; set; }
    public int CurrentLevel { get; set; }
    //[Header("User Settings")]
    public float SensitivityMultiplier { get; set; }
    public float MusicVol { get; set; }
    public float SFXVol { get; set; }



// public int Points;
    public bool ChallengeReqAchieved { get; set; }
    public bool LastChallenge { get; set; }


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

        //Get all persistant values from registry on load
        //Level
        if (PlayerPrefs.HasKey("Level"))
        {
            Level = PlayerPrefs.GetInt("Level");
        }
        else
        {
            Level = 1;
        }
        //Challenge
        if (PlayerPrefs.HasKey("ChallengeLevel"))
        {
            ChallengeLevel = PlayerPrefs.GetInt("ChallengeLevel");
        }
        else
        {
            ChallengeLevel = 1;
        }
        //Sensitivity
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            SensitivityMultiplier = PlayerPrefs.GetFloat("Sensitivity");
        }
        else
        {
            SensitivityMultiplier = 1.5f;
        }
        //Music Vol
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            MusicVol = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            MusicVol = 0.5f;
        }
        //SFX Vol
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

    public bool IsCurrentChallengeUnlocked()
    {
        if (CurrentChallenge <= ChallengeLevel)
        {
            return true;
        }
        else
        {
           return false;
        }

    }


    public void ChallengeCompleted()
    {
        //if this was the last challenge, and we're not replaying an old level - unlock the next one
        if (CurrentLevel == Level)
        {
            if (LastChallenge)
            {
                LevelUnlocked();
            }
            else
            {
                //set it
                SaveVal("ChallengeLevel", ++ChallengeLevel);
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
        //save it
        SensitivityMultiplier = s;
        SaveVal("Sensitivity", s);
    }

    public void SetMusicVolume(float vol)
    {
        //save it
        MusicVol = vol;
        SaveVal("MusicVolume", vol);
    }

    public void SetSFXVolume(float vol)
    {
        //save it
        SFXVol = vol;
        SaveVal("SFXVolume", vol);
    }


    private void SaveVal(string variableName, float value)
    {
        PlayerPrefs.SetFloat(variableName, value);
        PlayerPrefs.Save();
    }
    private void SaveVal(string variableName, int value)
    {
        PlayerPrefs.SetInt(variableName, value);
        PlayerPrefs.Save();
    }
}

using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Challenge : MonoBehaviour
{
    [Header("Global Objects")]
    public GameObject Ball;
    public GameObject Objectives;
    public GameObject ShotCompleteUI;
    public GameObject overlayManager;
    public GameObject HintsManager;
    public GameObject AdManager;

    [Space]
    [Header("Challenge Info")]
    public int LevelNumber;
    public int TotalChallenges;
    protected int ChallengesUnlocked;

    [SerializeField]
    [Header("Start Positions")]
    protected Vector3[] ballPosition;
    [SerializeField]
    protected Vector3[] ballRotation;

    [SerializeField]
    [Header("Challenge Solutions")]
    protected Vector2[] solutionAngle;
    [SerializeField]
    protected float[] solutionPower;
    protected GameObject[] ChallengeLocked;

    [Header("Challenge UI")]
    public GameObject[] ObjectiveUI;
    public GameObject[] ChallengeLockedUI;

    protected float ChallengeTransitionTimef = 2.5f;

    protected GameManager GM;

    public void Awake()
    {
        //start loading the banner ad
        AdManager.GetComponent<Adverts>().PlayBannerAd();

        //get game manager instance
        GM = GameManager.Instance;

        //set sfx audio level
        GameObject.Find("Canvas").GetComponent<AudioSource>().volume = GameManager.Instance.SFXVol;

        //Hide current objectives
        HideObjectives(GM.CurrentChallenge);

        //set our level values
        SetLevelValues();

        //update the padlocks for this level (called from inherited class)
        UpdatePadlocks();

        //store the solutions for this level in the movement script for the ball to use
        SetSolutions();

        //update camera position
        Camera.main.GetComponent<Animator>().SetInteger("CurrentChallenge", GM.CurrentChallenge);

        //set up the challenge
        if (GM.CurrentChallenge > 1)
        {
            PlayLevelChange();
           // StartCoroutine(SetNewChallenge(ChallengeTransitionTimef));
        }
    }


    public void PlayNextChallenge()
    {
        if (GameManager.Instance.CurrentChallenge < TotalChallenges && !Ball.GetComponent<Movement>().IsLiveShot)
        {
            //clear any current overlay
            overlayManager.GetComponent<OverlayManager>().ClearOverlay();

            //inc the current challenge
            GM.CurrentChallenge++;
            //ChallengesUnlocked++;

            PlayLevelChange();
        }
    }

    public void PlayPreviousChallenge()
    {
        if (GM.CurrentChallenge > 1 && !Ball.GetComponent<Movement>().IsLiveShot)
        {
            //clear any current overlay
            overlayManager.GetComponent<OverlayManager>().ClearOverlay();

            //inc the current challenge
            GM.CurrentChallenge--;

            PlayLevelChange();
        }
    }

    public void PlayLevelChange()
    {
        //stop celebrating if we're moving on from previous challenge
        ShotCompleteUI.GetComponent<UIShotComplete>().StopCelebrating();

        //hide the UI buttons
        ShotCompleteUI.GetComponent<RawImage>().enabled = false;
        //reset the UI shot compelte scale ready for the next animation
        if (ShotCompleteUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("NailedIt"))
        {
            ShotCompleteUI.GetComponent<Animator>().SetTrigger("Reset");
        }
        //hide the objectives
        HideObjectives();
        //hide hints
        if (HintsManager) { HintsManager.GetComponent<Hints>().HideHints(); }

        //update camera animation
        Camera.main.GetComponent<Animator>().SetInteger("CurrentChallenge", GM.CurrentChallenge);

    }

    public void SetNewChallenge()
    {
        if (GM.CurrentChallenge <= ChallengesUnlocked)
        {
            //unlock the challenge
            UpdatePadlocks();

            //call our overlay for popups on the current challenge
            overlayManager.GetComponent<OverlayManager>().ShowOverlay();

            //limit calls for GM
            int ChallengeNoArr = GM.CurrentChallenge - 1;

            Ball.GetComponent<Ball>().SetSpawnPoint(ballPosition[ChallengeNoArr], ballRotation[ChallengeNoArr]);

            //spawn the ball
            Ball.GetComponent<Ball>().Respawn(true, 0f);

            //unhide the challenge UI
            HideObjectives(ChallengeNoArr + 1);

            //update hints
            if (HintsManager) { HintsManager.GetComponent<Hints>().UpdateHint(); }
        }
    }


    protected void UpdatePadlocks()
    {
        for (int i = 0; i < ChallengesUnlocked; i++)
        {
            if (ChallengeLockedUI[i])
            {
               // ChallengeLockedUI[i].GetComponent<LockBounce>().Show = false;
                ChallengeLockedUI[i].gameObject.SetActive(false);
            }

        }
    }

    protected void UpdatePadlock(int i)
    {
       // ChallengeLockedUI[i].GetComponent<LockBounce>().Show = false;
        ChallengeLockedUI[i].gameObject.SetActive(false);
    }

    public void SetCurrentChallenge(int i)
    {
        GM.CurrentChallenge = i;
    }

    public void SetCurrentLevel(int i)
    {
        GM.CurrentLevel = i;
    }

    public void AddChallengeUnlocked()
    {
        if (ChallengesUnlocked < TotalChallenges)
        {
            //increment challenges unlocked
            ChallengesUnlocked++;
            //update that padlock
            UpdatePadlock(ChallengesUnlocked - 1);
        }

    }

    public void HideObjectives(int exception = 0)
    {
        int i = 1;
        foreach (Transform child in Objectives.GetComponent<Transform>())
        {
            if (i != exception)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
            i++;
        }
    }


    protected void SetLevelValues()
    {
        //if the level we're on is fully unlocked, unlock all challenges
        if (GM.Level > LevelNumber)
        {
            //set the unlocked challenges
            ChallengesUnlocked = TotalChallenges;
        }
        else
        {
            //otherwise pull the number from the saved data
            ChallengesUnlocked = GM.ChallengeLevel;
        }


    }


    protected void SetSolutions()
    {
        //store the solutions in the movement script for the ball to use
        Ball.GetComponent<Movement>().SetSolutionValues(solutionAngle, solutionPower);
    }
}

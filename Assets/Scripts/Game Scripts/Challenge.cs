using UnityEngine;
using UnityEngine.UI;


public class Challenge : MonoBehaviour
{
    [Header("Global Objects")]
    [SerializeField]
    protected GameObject Ball;
    [SerializeField]
    protected GameObject Objectives;
    [SerializeField]
    protected GameObject ShotCompleteUI;
    [SerializeField]
    protected GameObject overlayManager;
    [SerializeField]
    protected GameObject HintsManager;
    [SerializeField]
    protected GameObject AdManager;

    [Space]
    [Header("Challenge Info")]
    [SerializeField]
    protected int LevelNumber;
    public int TotalChallenges;
    protected int ChallengesUnlocked;

    [Header("Start Positions")]
    [SerializeField]
    protected Vector3[] ballPosition;
    [SerializeField]
    protected float[] ballRotation;

    [Header("Challenge Solutions")]
    [SerializeField]
    protected Vector2[] solutionAngle;
    [SerializeField]
    protected float[] solutionPower;
    protected GameObject[] ChallengeLocked;

    [Header("Challenge UI")]
    public GameObject[] ObjectiveUI;
    [SerializeField]
    protected GameObject[] ChallengeLockedUI;

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
    }


    public void PlayNextChallenge()
    {
        if (GameManager.Instance.CurrentChallenge < TotalChallenges && !Ball.GetComponent<Movement>().IsLiveShot)
        {
            //clear any current overlay
            overlayManager.GetComponent<OverlayManager>().ClearOverlay();

            //inc the current challenge
            GM.CurrentChallenge++;

            //set up the level
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

            //set up the level
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

    //Called from animation event after camera pan.
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

            Ball.GetComponent<Ball>().SetSpawnPoint(ballPosition[ChallengeNoArr], new Vector3(0f,ballRotation[ChallengeNoArr],0f));

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
        ChallengeLockedUI[i].gameObject.SetActive(false);
    }

    //utilised by UI buttons
    public void SetCurrentChallenge(int i)
    {
        GM.CurrentChallenge = i;
    }

    //utilised by UI buttons
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
        //loop through all objectives under the objective manager and hide unless exception is specified (>0)
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

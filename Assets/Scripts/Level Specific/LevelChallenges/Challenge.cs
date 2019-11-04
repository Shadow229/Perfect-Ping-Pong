using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Challenge : MonoBehaviour
{
    [Header("Global Objects")]
    public GameObject Ball;
    public GameObject Objectives;
    public GameObject ShotCompleteUI;
    public GameObject overlayManager;

    [Space]
    [Header("Challenge Info")]
    public int LevelNumber;
    public int TotalChallenges;
    public int ChallengesUnlocked;

    [SerializeField]
    [Header("Start Positions")]
    protected Vector3[] ballPosition;
    [SerializeField]
    protected Vector3[] ballRotation;

    [SerializeField]
    [Header("Challenge Solutions")]
    protected Vector2[] solutionAngle;
    [SerializeField]
    protected Vector3[] solutionVelocity;
    protected GameObject[] ChallengeLocked;

    [Header("Challenge UI")]
    public GameObject[] ObjectiveUI;
    public GameObject[] ChallengeLockedUI;

    protected float ChallengeTransitionTimef;


    public void PlayNextChallenge()
    {
        if (GameManager.Instance.CurrentChallenge < TotalChallenges)
        {
            //clear any current overlay
            overlayManager.GetComponent<OverlayManager>().ClearOverlay();

            //inc the current challenge
            GameManager.Instance.CurrentChallenge++;
            //ChallengesUnlocked++;

            //hide the UI buttons
            ShotCompleteUI.GetComponent<RawImage>().enabled = false;
            HideObjectives();

            //update camera animation
            Camera.main.GetComponent<Animator>().SetInteger("CurrentChallenge", GameManager.Instance.CurrentChallenge);

            //wait for the camera animation to go to the next challenge
            // if its unlocked, set it up
            if (GameManager.Instance.CurrentChallenge <= ChallengesUnlocked)
            {
                StartCoroutine(SetNewChallenge(ChallengeTransitionTimef));
            }

        }

        //load the main menu?
    }

    public void PlayPreviousChallenge()
    {
        if (GameManager.Instance.CurrentChallenge > 1)
        {
            //clear any current overlay
            overlayManager.GetComponent<OverlayManager>().ClearOverlay();

            //inc the current challenge
            GameManager.Instance.CurrentChallenge--;
            //ChallengesUnlocked++;

            //hide the UI buttons
            ShotCompleteUI.GetComponent<RawImage>().enabled = false;
            HideObjectives();

            //update camera animation
            Camera.main.GetComponent<Animator>().SetInteger("CurrentChallenge", GameManager.Instance.CurrentChallenge);

            //wait for the camera animation to go to the next challenge
            // if its unlocked, set it up
            if (GameManager.Instance.CurrentChallenge <= ChallengesUnlocked)
            {
                StartCoroutine(SetNewChallenge(ChallengeTransitionTimef));
            }
        }   
    }

    protected IEnumerator SetNewChallenge(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //unlock the challenge
        UpdatePadlocks();

        //call our overlay for popups on the current challenge
        overlayManager.GetComponent<OverlayManager>().ShowOverlay();

        //move the ball (and trajectories)
        //Ball.GetComponent<Ball>().ZeroOut();
        //Ball.transform.position = ballPosition[GameManager.Instance.CurrentChallenge - 1];
        //Ball.transform.eulerAngles = ballRotation[GameManager.Instance.CurrentChallenge - 1];
        //Ball.GetComponent<Ball>().ZeroOut();

        Ball.GetComponent<Ball>().SetSpawnPoint(ballPosition[GameManager.Instance.CurrentChallenge - 1], ballRotation[GameManager.Instance.CurrentChallenge - 1]);

        //spawn the ball
        Ball.GetComponent<Ball>().Respawn(true, 0f);

        //unhide the challenge UI
        HideObjectives(GameManager.Instance.CurrentChallenge);
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
        GameManager.Instance.CurrentChallenge = i;
    }

    public void SetCurrentLevel(int i)
    {
        GameManager.Instance.CurrentLevel = i;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialChallenges : MonoBehaviour
{

    [Header("Challenge Info")]
    public int CurrentChallenge = 1;
    public int TotalChallenges = 3;

    [Space]
    [Header("Global Objects")]
    public GameObject Ball;
    public GameObject Objectives;
    public float ChallengeTransitionTime = 3.5f;
    public Vector3[] ballPosition;
    public Vector3[] ballRotation;

    [Space]
    [Header("Challenge 1:")]
    public GameObject C1Objective1;

    [Space]
    [Header("Challenge 2:")]
    public GameObject C2Objective1;
    public GameObject C2LockedUI;

    [Space]
    [Header("Challenge 3:")]
    public GameObject C3LockedUI;

    private int ChallengesUnlocked;

    private bool Achieved = false;

    public void Start()
    {
        if (GameManager.Instance.Level > 1)
        {
            ChallengesUnlocked = TotalChallenges;
        }
        else
        {
            ChallengesUnlocked = GameManager.Instance.ChallengeLevel;
        }

        updatePadlocks();
    }


    //update padlockes
    private void updatePadlocks()
    {
        for (int i = 0; i < ChallengesUnlocked; i++)
        {
            switch (i)
            {
                case 0:
                    break;
                case 1:
                    C2LockedUI.SetActive(false);
                    break;
                case 2:
                    C3LockedUI.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }

    public void Update()
    {
        switch (CurrentChallenge)
        {
            case 1:
                Achieved = Level1();
                break;

            case 2:
                Achieved = Level2();
                break;

            case 3:
                Achieved = Level3();
                break;

            default:
                Achieved = false;
                break;
        }

        //tell the game manager the outcome then reset the achieved flag
        GameManager.Instance.LevelReqAchieved = Achieved;
        Achieved = false;

        //update the camera animation parameter

    }

    private bool Level1()
    {
        return true;
    }

    private bool Level2()
    {

        if (Ball.GetComponent<Movement>().BounceCount >= 1)
        {
            C2Objective1.transform.GetChild(0).GetComponent<Image>().enabled = true;
            return true;
        }

        C2Objective1.transform.GetChild(0).GetComponent<Image>().enabled = false;
        return false;
    }

    private bool Level3()
    {
        return false;
    }

    public void HideObjectives(int exception = 0)
    {
        int i = 0;
        foreach (Transform child in Objectives.transform)
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


    public void PlayNextChallenge()
    {
        //inc the current challenge
        CurrentChallenge++;
        ChallengesUnlocked++;

        //hide the UI buttons
        HideObjectives();

        //update camera animation
        Camera.main.GetComponent<Animator>().SetInteger("Level", CurrentChallenge);

        //wait for the camera animation to go to the next challenge
        StartCoroutine(SetNewChallenge(ChallengeTransitionTime));

    }

    private IEnumerator SetNewChallenge(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //unlock the challenge
        updatePadlocks();

        //unhide the challenge UI
        HideObjectives(CurrentChallenge);

        //move the ball (and trajectories)
        Ball.GetComponent<Ball>().ZeroOut();
        Ball.transform.position = ballPosition[CurrentChallenge - 1];
        Ball.transform.eulerAngles = ballRotation[CurrentChallenge - 1];

        Ball.GetComponent<Ball>().SetSpawnPoint(ballPosition[CurrentChallenge - 1], ballRotation[CurrentChallenge - 1]);
    }
}

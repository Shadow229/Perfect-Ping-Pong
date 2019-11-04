﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialChallenges : Challenge
{
    private bool Achieved = false;

    public void Awake()
    {
        //
        HideObjectives(GameManager.Instance.CurrentChallenge);

        //set our level values
        SetLevelValues();

       //update the padlocks for this level (called from inherited class)
        UpdatePadlocks();

        //store the solutions for this level in the movement script for the ball to use
        SetSolutions();

        //update camera position
        Camera.main.GetComponent<Animator>().SetInteger("CurrentChallenge", GameManager.Instance.CurrentChallenge);

        //set up the challenge
        StartCoroutine(SetNewChallenge(ChallengeTransitionTimef));
    }

    public void Update()
    {
        //if the ball has been thrown start checking if the criteria have been met to complete the challenge
        if (!Ball.GetComponent<Movement>()._Began)
        {
            switch (GameManager.Instance.CurrentChallenge)
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
        }

        //tell the game manager the outcome then reset the achieved flag
        GameManager.Instance.ChallengeReqAchieved = Achieved;
        Achieved = false;

    }


    private void SetLevelValues()
    {
        //challenge values
        LevelNumber = 1;
        //CurrentChallenge = 1;
        TotalChallenges = 3;
        //camera movement
        ChallengeTransitionTimef = 2.5f;

        //if the level we're on is fully unlocked, unlock all challenges
        if (GameManager.Instance.Level > LevelNumber)
        {
            //set the unlocked challenges
            ChallengesUnlocked = TotalChallenges;
        }
        else
        {
            //otherwise pull the number from the saved data
            ChallengesUnlocked = GameManager.Instance.ChallengeLevel;
        }


    }


    private void SetSolutions()
    {
        //Vector2 Ang1, Ang2, Ang3;
        //Vector3 Vel1, Vel2, Vel3;

        ////Solution for challenge1
        //Ang1 = new Vector2(-15.9f, 51.5f);
        //Vel1 = new Vector3(-68.1f, 313.1f, 239.5f);

        ////Solution for challenge2
        //Ang2 = new Vector2(286.41f, 73.19f);
        //Vel2 = new Vector3(-94.32f, 325.47f, 27.79f);

        ////Solution for challenge3
        //Ang3 = new Vector2(-13f, 48.85f);
        //Vel3 = new Vector3(-86f, 436.75f, 371.83f);

        //store the solutions in the movement script for the ball to use
        Ball.GetComponent<Movement>().SolutionAngle = solutionAngle;
        Ball.GetComponent<Movement>().SolutionVelocity = solutionVelocity;
    }


 

    //Challenge logics//

    //Level 1: no challenge - just get the ball in
    private bool Level1()
    {
        GameManager.Instance.lastChallenge = false;

        if (GameManager.Instance.ChallengeLevel <= 1 && GameManager.Instance.Level <= LevelNumber)
        {
            return true;
        }
        else
        {
            return ShowChallengeComplete(true);
        }
    }

    //Level 2: 1 or more bounces
    private bool Level2()
    {
        GameManager.Instance.lastChallenge = false;

        if (GameManager.Instance.ChallengeLevel <= 2 && GameManager.Instance.Level <= LevelNumber)
        {
            if (Ball.GetComponent<Movement>().BounceCount >= 1)
            {
                return ShowChallengeComplete(true);
            }

            return ShowChallengeComplete(false);            
        }
        else
        {
            return ShowChallengeComplete(true);
        }
 
    }

    //Level 3: 1 or more rebounds
    private bool Level3()
    {
        GameManager.Instance.lastChallenge = true;

        if (GameManager.Instance.ChallengeLevel <= 3 && GameManager.Instance.Level <= LevelNumber)
        {
            if (Ball.GetComponent<Movement>().ReboundCount >= 1)
            {
                return ShowChallengeComplete(true);
            }
            else
            {
                return ShowChallengeComplete(false);
            }
        }
        else
        {
            return ShowChallengeComplete(true);
        }
    }

    private bool ShowChallengeComplete(bool val)
    {
        ObjectiveUI[GameManager.Instance.CurrentChallenge - 1].transform.GetChild(0).GetComponent<Image>().enabled = val;
        return val;
    }
}

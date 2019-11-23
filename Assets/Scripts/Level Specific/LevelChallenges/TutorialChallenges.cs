using UnityEngine.UI;

public class TutorialChallenges : Challenge
{
    private bool Achieved = false;

    public void Update()
    {
        //if the ball has been thrown start checking if the criteria have been met to complete the challenge
        if (!Ball.GetComponent<Movement>().HasBegan)
        {
            switch (GM.CurrentChallenge)
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
        GM.ChallengeReqAchieved = Achieved;
        Achieved = false;



    }


    //Challenge logics//

    //Level 1: no challenge - just get the ball in
    private bool Level1()
    {
        GM.LastChallenge = false;

        if (GM.ChallengeLevel <= 1 && GM.Level <= LevelNumber)
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
        GM.LastChallenge = false;

        if (GM.ChallengeLevel <= 2 && GM.Level <= LevelNumber)
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
        GM.LastChallenge = true;

        if (GM.ChallengeLevel <= 3 && GM.Level <= LevelNumber)
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

    //enable the complete UI
    private bool ShowChallengeComplete(bool val)
    {
        ObjectiveUI[GM.CurrentChallenge - 1].transform.GetChild(0).GetComponent<Image>().enabled = val;
        return val;
    }
}
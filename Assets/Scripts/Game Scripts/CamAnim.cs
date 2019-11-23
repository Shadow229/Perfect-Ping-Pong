using UnityEngine;

public class CamAnim : MonoBehaviour
{
    public GameObject ChallengeManager;
    public GameObject ChallengeMovement;

    //Function called from the end of the camera animation to set up the shot
    public void SetNextChallenge(int ChalNo)
    {
        //if the challenge event called is the challenge we're still on (aka, we havent continued skipping challenges), run the set up
        if (ChalNo == GameManager.Instance.CurrentChallenge)
        {
            ChallengeManager.GetComponent<Challenge>().SetNewChallenge();
        }

    }

    public void ResetChallengeSelectorCounters()
    {
        ChallengeMovement.GetComponent<ChallengeSelector>().ResetCounters();
    }
}

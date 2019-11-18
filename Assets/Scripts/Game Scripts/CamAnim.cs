using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAnim : MonoBehaviour
{
    public GameObject ChallengeManager;

    //Function called from the end of the camera animation to set up the shot
    public void SetNextChallenge(int ChalNo)
    {
        if (ChalNo == GameManager.Instance.CurrentChallenge)
        {
            ChallengeManager.GetComponent<Challenge>().SetNewChallenge();
        }
    }
}

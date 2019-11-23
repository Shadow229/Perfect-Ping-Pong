using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeSelectorBtn : MonoBehaviour
{
    public bool Next = false;
    public GameObject ChallengeManager;
    public GameObject ChallengeMovement;
    public int clickAmt = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);   
    }

    private void TaskOnClick()
    {
        clickAmt++;

        if (Next)
        {
            ChallengeManager.GetComponent<Challenge>().PlayNextChallenge();
        }
        else
        {
            ChallengeManager.GetComponent<Challenge>().PlayPreviousChallenge();
        }

        ChallengeMovement.GetComponent<ChallengeSelector>().UpdateVisibility();

        ChallengeMovement.GetComponent<ChallengeSelector>().UpdateUIChallengeSwap();
    }

    public void ZeroClickCounter()
    {
        clickAmt = 0;
    }
}

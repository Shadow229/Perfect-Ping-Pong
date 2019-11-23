using UnityEngine;
using UnityEngine.UI;

public class ChallengeSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject ChallengeManager = null;
    [SerializeField]
    private GameObject Next = null, Previous = null;

    public void Awake()
    {
        if (GameManager.Instance.CurrentChallenge == 1)
        {
            UpdateVisibility();
        }
    }

    //show and hide next and previous challenge buttons depending on the current challenge and total challenges in level
    public void UpdateVisibility()
    {
        //if its the last challenge for the next button
        if (ChallengeManager && Next)
        {
            if (ChallengeManager.GetComponent<Challenge>().TotalChallenges == GameManager.Instance.CurrentChallenge)
            {
                Next.GetComponent<Button>().enabled = false;
                Next.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
            Next.transform.GetChild(0).gameObject.SetActive(true);
                Next.GetComponent<Button>().enabled = true;
            }
        }

        if (Previous)
        {
            //if its the first challenge for the previous button
            if (GameManager.Instance.CurrentChallenge == 1)
            {
                Previous.GetComponent<Button>().enabled = false;
                Previous.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                Previous.GetComponent<Button>().enabled = true;
                Previous.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    public void ResetCounters()
    {
        Next.GetComponent<ChallengeSelectorBtn>().ZeroClickCounter();
        Previous.GetComponent<ChallengeSelectorBtn>().ZeroClickCounter();
    }


    public void UpdateUIChallengeSwap()
    {
        //if the player has moved quickly back and forth on challenges, reload the objectives
        if (Next.GetComponent<ChallengeSelectorBtn>().clickAmt - Previous.GetComponent<ChallengeSelectorBtn>().clickAmt == 0)
        {
            //update the challenges
            ChallengeManager.GetComponent<Challenge>().SetNewChallenge(); ;
        }
    }
}

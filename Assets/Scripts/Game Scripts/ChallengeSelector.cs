//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class ChallengeSelector : MonoBehaviour
{
    public GameObject ChallengeManager;
    public GameObject Next, Previous;

    public void Awake()
    {
        UpdateVisibility();
    }

    public void UpdateVisibility()
    {
        //if its the last challenge for the next button
        if (ChallengeManager && Next)
        {
            if (ChallengeManager.GetComponent<Challenge>().TotalChallenges == GameManager.Instance.CurrentChallenge)
            {
                Next.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
            Next.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        if (Previous)
        {
            //if its the first challenge for the previous button
            if (GameManager.Instance.CurrentChallenge == 1)
            {
                Previous.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                Previous.transform.GetChild(0).gameObject.SetActive(true);
            }
        } 
    }
}

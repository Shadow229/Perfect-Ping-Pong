//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}

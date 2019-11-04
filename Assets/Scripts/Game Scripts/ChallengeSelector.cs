using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeSelector : MonoBehaviour
{
    public GameObject ChallengeManager;
    public bool NextSelector;


    public void Awake()
    {
        UpdateVisibility();
    }


    public void UpdateVisibility()
    {


        //if its the 'next' button
        if (NextSelector)
        {
            //sense check on the challenge manager
            if (!ChallengeManager) return;

            if (ChallengeManager.GetComponent<Challenge>().TotalChallenges == GameManager.Instance.CurrentChallenge)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            if (GameManager.Instance.CurrentChallenge == 1)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
       
    }
}

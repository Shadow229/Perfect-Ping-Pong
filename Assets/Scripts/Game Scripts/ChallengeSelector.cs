using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeSelector : MonoBehaviour
{
    public GameObject ChallengManager;
    public bool NextSelector;
    public void UpdateVisibility()
    {
        //if its the 'next' button
        if (NextSelector)
        {
            if (ChallengManager.GetComponent<Challenge>().TotalChallenges == GameManager.Instance.CurrentChallenge)
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

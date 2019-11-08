using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShotComplete : MonoBehaviour
{
    public AnimationCurve FinishUIcurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

    public GameObject MenuBtn;
    public GameObject NextChallengeBtn;
    public GameObject NextLevelBtn;

    //private bool ShotSuccess = false;
    private RectTransform rt;

    public void PlaySplashUI()
    {

        rt = GetComponent<RectTransform>();

        //mark the shot as a success for the update function
        StartCoroutine(MarkSuccessful(0.8f));
    }


    private IEnumerator MarkSuccessful(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        ShotSuccess();
    }


    private void ShotSuccess()
    {
        Animator anim = GetComponent<Animator>();

        //start the animation
        if (anim)
        {
            anim.Play("NailedIt"); //getting stuck here. need to reset it when one of the buttons below is hit
        };

        //show the splash completed image
        if (!GetComponent<RawImage>().enabled)
        {
            GetComponent<RawImage>().enabled = true;

        }


        //when the animation has finished, pop up the UI for next level

        //show proceed buttons
        MenuBtn.SetActive(true);

        if (!GameManager.Instance.lastChallenge)
        {
            NextChallengeBtn.SetActive(true);
        }
        else if (GameManager.Instance.CurrentLevel < GameManager.Instance.MaxLevels)
        {
            NextLevelBtn.SetActive(true);
        }
    }
}

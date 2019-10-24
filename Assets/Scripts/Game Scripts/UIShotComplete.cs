using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShotComplete : MonoBehaviour
{
    public AnimationCurve FinishUIcurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

    public Vector3 startScale, endScale;
    public float duration = 1f;
 

    public GameObject MenuBtn;
    public GameObject NextChallengeBtn;

    private bool ShotSuccess = false;
    private RectTransform rt;

    private float time = 0f;
    private float pos = 0f;

    public void PlaySplashUI()
    {

        rt = GetComponent<RectTransform>();

        //show the splash 'well done' image -- This is being disabled as a game object by the 'next challenge' button in the UI
        if (!GetComponent<RawImage>().enabled)
        {
            GetComponent<RawImage>().enabled = true;
        }

        //mark the shot as a success for the update function
        StartCoroutine(MarkSuccessful(0.8f));
    }


    private IEnumerator MarkSuccessful(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        ShotSuccess = true;
    }


    private void Update()
    {
        if (ShotSuccess)
        {
            //change the scale of the splash text over time
            time += Time.deltaTime;
            pos = time / duration;
            rt.localScale = Vector3.Lerp(startScale, endScale, FinishUIcurve.Evaluate(pos));
        }

        //when the animation has finished, pop up the UI for next level
        if(pos >= 1)
        {
            //reset the UI splash
            ShotSuccess = false;
            time = 0;
            pos = 0;

            //show proceed buttons
            MenuBtn.SetActive(true);
            NextChallengeBtn.SetActive(true);
        }

    }
}

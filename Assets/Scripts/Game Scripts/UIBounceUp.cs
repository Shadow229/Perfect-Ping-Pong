using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShotComplete : MonoBehaviour
{
    public AnimationCurve FinishUIcurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

    public Vector3 startScale, endScale;
    public float duration = 5f;
    public float time = 0f;
    public float pos = 0f;

    public GameObject MenuButton;
    public GameObject NextLvlButton;

    private bool ShotSuccess = false;
    private RectTransform rt;

    public void PlaySplashUI()
    {

        rt = GetComponent<RectTransform>();

        //show the image
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
            //change the scale over time
            time += Time.deltaTime;
            pos = time / duration;
            rt.localScale = Vector3.Lerp(startScale, endScale, FinishUIcurve.Evaluate(pos));
        }

        //when the animations finished, pop up the UI for next level
        if(pos >= 1)
        {
            MenuButton.SetActive(true);
            NextLvlButton.SetActive(true);
        }

    }
}

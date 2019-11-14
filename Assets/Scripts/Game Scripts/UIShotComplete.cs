using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShotComplete : MonoBehaviour
{
    public GameObject MenuBtn;
    public GameObject NextChallengeBtn;
    public GameObject NextLevelBtn;
    public AudioClip Clapping, Showing;


    private void Awake()
    {
        GetComponent<AudioSource>().volume = GameManager.Instance.SFXVol;
    }

    public void PlaySplashUI()
    {
        //give the player a round of applause
        GetComponent<AudioSource>().PlayOneShot(Clapping);

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
            anim.Play("NailedIt");
            GetComponent<AudioSource>().PlayOneShot(Showing);
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

    public void StopCelebrating()
    {
        AudioSource aud = GetComponent<AudioSource>();
        if (aud.isPlaying)
        {
            StartCoroutine(FadeOut(aud, 1f));
        }
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}

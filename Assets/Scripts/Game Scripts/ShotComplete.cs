using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShotComplete : MonoBehaviour
{
    public GameObject FinishSplash;
    public GameObject ChallengeManager;

    private bool Ran = false;

    private void OnTriggerEnter(Collider other)
    {
        //ensure its the ball we're colliting with and that its not a second bouce (only run once)
        if (other.CompareTag("Ball"))
        {

            ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
            Animator anim = GetComponent<Animator>();

            //wobble the cub
            if (anim)
            {
                anim.Play("Wobble");
            };

            //play the splash
            if (!ps.isPlaying)
            {
                ps.Play();
            }

            //stop bouncing
            other.GetComponent<SphereCollider>().material.bounciness = 0f;
            //zero out velocity - keep us in the target 
            other.GetComponent<Ball>().ZeroOut();



            //if the requirements for that challenge have been filled
            if (GameManager.Instance.ChallengeReqAchieved)
            {
                //only increment levels and such once
                if(!Ran)
                {
                    Ran = true;
                    //update the UI strikethrough for that challenge perminatently
                    ChallengeManager.GetComponent<Challenge>().ObjectiveUI[GameManager.Instance.CurrentChallenge - 1].transform.GetChild(0).GetComponent<Image>().enabled = true;

                    //update unlocked challenges
                    ChallengeManager.GetComponent<Challenge>().AddChallengeUnlocked();
                    //Save our incremented challenge level
                    GameManager.Instance.ChallengeCompleted();
                }
                else
                {
                    //Splash finish UI and buttons
                    FinishSplash.GetComponent<UIShotComplete>().PlaySplashUI();
                   // other.GetComponent<Ball>().Respawn(true);
                }

            }
            else
            {
                //initialse the reset
                other.GetComponent<Ball>().Respawn(true);
            }
            
            
        }
    }
}

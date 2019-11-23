using UnityEngine;
using UnityEngine.UI;


public class ShotComplete : MonoBehaviour
{
    [SerializeField]
    private GameObject FinishSplash = null;
    [SerializeField]
    private GameObject ChallengeManager = null;
    [SerializeField]
    private AudioClip Wobble = null, Splash = null;

    private bool Ran = false;


    private void Awake()
    {
        GetComponent<AudioSource>().volume = GameManager.Instance.SFXVol;
    }

    private void OnTriggerEnter(Collider other)
    {
        //ensure its the ball we're colliting with and that its not a second bouce (only run once)
        if (other.CompareTag("Ball"))
        {
            //get components
            ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
            Animator anim = GetComponent<Animator>();
            AudioSource aud = GetComponent<AudioSource>();

            //play the splash
            if (!aud.isPlaying)
            {
                aud.PlayOneShot(Splash);
                aud.PlayOneShot(Wobble);
            }

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

                //Splash finish UI and buttons
                FinishSplash.GetComponent<UIShotComplete>().PlaySplashUI();
            }
            else
            {
                //initialse the reset
                other.GetComponent<Ball>().Respawn(true);
            }
        }
    }
}

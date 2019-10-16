using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;


public class ShotComplete : MonoBehaviour
{
    public GameObject FinishSplash;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball" && GameManager.Instance.LevelReqAchieved)
        {
            ParticleSystem ps = GetComponentInChildren<ParticleSystem>();

            if (!ps.isPlaying)
            {
                ps.Play();
            }

            //stop bouncing
            other.GetComponent<SphereCollider>().material.bounciness = 0f;
            //keep us in the target
            other.GetComponent<Ball>().ZeroOut();
            //initialse the reset
            //other.GetComponent<Ball>().Respawn(true);

            //Splash finish UI and buttons
            FinishSplash.GetComponent<UIBounceUp>().PlaySplashUI();

        }
    }
}

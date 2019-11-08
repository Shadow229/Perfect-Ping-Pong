using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Vector3 SpawnPoint;
    public Vector3 SpawnRot;

    [SerializeField]
    [Range(0, 1)]
    float Bouncyness = 0.85f;

    bool Respawning = false;

    private float fDefaultBouncyness;

    // Start is called before the first frame update
    void Start()
    {
        fDefaultBouncyness = Bouncyness;
        SetSpawnPoint(transform.position, transform.eulerAngles);
    }

    public void SetSpawnPoint(Vector3 pos, Vector3 rot)
    {
        SpawnPoint = pos;
        SpawnRot = rot;
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    //Respawn();
    //}



    public void Respawn(bool ForceRespawn = false, float ForceTime = -1f)
    {
        if (!Respawning && (transform.position.y < -1.5 || ForceRespawn))
        {
            Respawning = true;
            //START A COROUTINE TO RESPAWN
            StartCoroutine(RespawnBall(ForceTime < 0 ? 2f : ForceTime)); ;
        }
    }

    private IEnumerator RespawnBall(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //mark it as a !Live shot
        GetComponent<Movement>().LiveShot = false;

        //reset our rb
        ZeroOut();

        //set our ball back
        transform.position =  SpawnPoint;
        transform.eulerAngles = SpawnRot;

        //incase of movement following the reset - zero out again
        ZeroOut();

        //put out bouncyness back on if its been removed
        GetComponent<SphereCollider>().material.bounciness = fDefaultBouncyness;

        //reset trajectory line
        GetComponent<Movement>().trajectoryLine.GetComponent<Trajectory>().ResetTrajectory();

        //reset our respawning flag
        Respawning = false;

        //mark the next shot at ready
        GetComponent<Movement>().SetReady = true;

        //reset the ball counters
        GetComponent<Movement>().BounceCount = 0;
        GetComponent<Movement>().ReboundCount = 0;

        //reset the bouncyness
        GetComponent<SphereCollider>().material.bounciness = Bouncyness;
    }

    public void ZeroOut()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

}

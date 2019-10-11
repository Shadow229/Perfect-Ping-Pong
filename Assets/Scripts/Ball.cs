using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Vector3 SpawnPoint = new Vector3(0.5f, 0f, -6.8f);

    [SerializeField]
    [Range(0, 1)]
    float Bouncyness = 0.85f;
    // Start is called before the first frame update

    [SerializeField]
    Movement movement;

    bool Respawning = false;

    private float fDefaultBouncyness;

    void Start()
    {
        fDefaultBouncyness = Bouncyness;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SphereCollider>().material.bounciness = Bouncyness;

        Respawn();
    }



    public void Respawn(bool ForceRespawn = false)
    {
        if (!Respawning && (transform.position.y < -1.5 || ForceRespawn))
        {
            Respawning = true;
            //START A COROUTINE TO RESPAWN
            StartCoroutine(RespawnBall(2f));
        }
    }

    private IEnumerator RespawnBall(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //reset our rb
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //set our ball back
        transform.position = SpawnPoint;

        //put out bouncyness back on if its been removed
        GetComponent<SphereCollider>().material.bounciness = fDefaultBouncyness;

        //reset our respawning flag
        Respawning = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReboundArea : MonoBehaviour
{
    public GameObject Ball;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball.GetComponent<Movement>().ReboundCount++;
        }
    }
}

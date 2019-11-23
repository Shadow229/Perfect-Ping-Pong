using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Dust : MonoBehaviour
{
    private void Start()
    {
        //delete itself after the particle effect has played out
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }
}

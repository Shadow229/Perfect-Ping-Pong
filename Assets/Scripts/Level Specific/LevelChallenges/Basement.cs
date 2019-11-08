using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basement : Challenge
{
    [Space]
    [Header("Challenge Info")]
    [SerializeField]
    private int levelNumber = 2;
    [SerializeField]
    private int totalChallenges = 1;

    private void Awake()
    {
        //set our level values
        SetLevelValues(levelNumber, totalChallenges);
    }
}

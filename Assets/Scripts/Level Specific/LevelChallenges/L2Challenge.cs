using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L2Challenge : MonoBehaviour
{
    public GameObject Ball;

    public GameObject ObjectiveStrike1;

    public void Update()
    {
        GameManager.Instance.LevelReqAchieved = Level2();
    }

    private bool Level2()
    {
        if (Ball.GetComponent<Movement>().BounceCount >= 1)
        {
            ObjectiveStrike1.GetComponent<Image>().enabled = true;
            return true;
        }

        ObjectiveStrike1.GetComponent<Image>().enabled = false;
        return false;
    }

}

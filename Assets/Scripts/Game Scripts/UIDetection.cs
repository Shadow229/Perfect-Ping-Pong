using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDetection : MonoBehaviour
{
    public GameObject Ball;

    public List<RaycastResult> RaycastTouch()
    {

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = Input.GetTouch(0).position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

       // Debug.Log(results.Count);

        return results;
    }

    public bool CheckUIpressed()
    {
        bool Overlapping = false;
        List<RaycastResult> results = RaycastTouch();

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results)
        {
            //Debug.Log("Hit " + result.gameObject.name);

            //if we're over the powerbar or hitting the finish UI buttons, dont throw the ball 
            if (result.gameObject.name == "PowerBar" || result.gameObject.name == "NextChallengeBtn" || result.gameObject.name == "MenuBtn" |
                // if we're hitting next or preview (while not throwing the ball already) dont throw the ball
                (!Ball.GetComponent<Movement>()._Began && (result.gameObject.name == "NextChallenge" || result.gameObject.name == "PrevChallenge")))
            // if we're pressing next challenge or menu dont throw it)
            {
                Overlapping = true;
                break;
            }
        }

        return Overlapping;
    }
}

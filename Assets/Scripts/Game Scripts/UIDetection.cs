using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDetection : MonoBehaviour
{
    [SerializeField]
    private GameObject Ball = null;
    [SerializeField]
    private GameObject overlayManager = null;

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
        //if theres a UI overlay dont move anything
        if (overlayManager.GetComponent<OverlayManager>().OverlayCheck[GameManager.Instance.CurrentChallenge - 1])
        {
            return true;
        }
        else
        {
            //get the results
            List<RaycastResult> results = RaycastTouch();

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                //Debug.Log("Hit " + result.gameObject.name);

                //if we're hitting the finish UI buttons, dont throw the ball 
                if (result.gameObject.CompareTag("UIProceed") ||
                    // if we're hitting next or preview (while not throwing the ball already) dont throw the ball
                    (!Ball.GetComponent<Movement>().HasBegan && result.gameObject.CompareTag("UISkipChallenge")))
                {
                    return true;
                }
            }
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDetection : MonoBehaviour
{

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

    public bool CheckPowerBar()
    {
        bool Overlapping = false;
        List<RaycastResult> results = RaycastTouch();

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results)
        {
            //Debug.Log("Hit " + result.gameObject.name);

            if (result.gameObject.name == "PowerBar")
            {
                Overlapping = true;
                break;
            }

        }

        return Overlapping;
    }
}

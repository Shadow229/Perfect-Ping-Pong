//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    public GameObject[] Overlays;

    public bool[] OverlayCheck { get; private set; }

    public void Awake()
    {
        //set the length of our bool arr
        OverlayCheck = new bool[Overlays.Length];

        //call our overlay for popups on the current challenge
        ShowOverlay();
    }


    public void ClearOverlay()
    {
        int arrChal = GameManager.Instance.CurrentChallenge - 1;
        //sense check
        if (Overlays[arrChal] && OverlayCheck[arrChal])
        {
            //clear it
            Overlays[arrChal].gameObject.SetActive(false);
            //remove it from the public boolean array as this is checked in UIDetection
            OverlayCheck[arrChal] = false;
        }
    }

    public void ShowOverlay()
    {
        int arrChal = GameManager.Instance.CurrentChallenge - 1;
        //sense check
        if (Overlays[arrChal] && !OverlayCheck[arrChal] && (GameManager.Instance.CurrentLevel < GameManager.Instance.Level || arrChal + 1 <= GameManager.Instance.ChallengeLevel))
        {
            //show it
            Overlays[arrChal].gameObject.SetActive(true);

            //add it back into the array for UI detection
            OverlayCheck[arrChal] = true;
        }
    }
}

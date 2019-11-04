using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    public GameObject[] Overlays;

    public bool[] _overlayVis;

    public void Awake()
    {
        //set the length of our bool arr
        _overlayVis = new bool[Overlays.Length];
        //set visability on active overlays
        //for (int i = 0; i < Overlays.Length; i++)
        //{
        //    if (Overlays[i])
        //    {
        //        _overlayVis[i] = true;
        //    }
        //}

        //call our overlay for popups on the current challenge
        ShowOverlay();
    }


    public void ClearOverlay()
    {
        int arrChal = GameManager.Instance.CurrentChallenge - 1;
        //sense check
        if (Overlays[arrChal] && _overlayVis[arrChal])
        {
            //clear it
            Overlays[arrChal].gameObject.SetActive(false);
            //remove it from the public boolean array as this is checked in UIDetection
            _overlayVis[arrChal] = false;
        }
    }

    public void ShowOverlay()
    {
        int arrChal = GameManager.Instance.CurrentChallenge - 1;
        //sense check
        if (Overlays[arrChal] && !_overlayVis[arrChal] && (GameManager.Instance.CurrentLevel < GameManager.Instance.Level || arrChal + 1 <= GameManager.Instance.ChallengeLevel))
        {
            //show it
            Overlays[arrChal].gameObject.SetActive(true);

            //add it back into the array for UI detection
            _overlayVis[arrChal] = true;
        }
    }
}

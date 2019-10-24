using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStrikethrough : MonoBehaviour
{
    public void UpdateLevelUI()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();

        int i = 1;

        foreach (Transform child in allChildren)
        {
            if (child.name == "Locked")
            {
                if (i <= GameManager.Instance.Level)
                {
                    child.gameObject.GetComponent<Image>().enabled = false;
                }
                else
                {
                    child.gameObject.GetComponent<Image>().enabled = true;
                }

                i++;

            }

        }
    }
    
}

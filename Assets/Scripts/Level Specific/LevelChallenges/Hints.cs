using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hints : MonoBehaviour
{
    public GameObject[] hints;

    private void Start()
    {
        UpdateHint();
    }

    public void UpdateHint()
    {
        for (int i = 0; i < hints.Length; i++)
        {
            if (i == GameManager.Instance.CurrentChallenge - 1)
            {
                hints[i].SetActive(true);
            }
            else
            {
                hints[i].SetActive(false);
            }
        }
    }

    public void HideHints()
    {
        foreach (GameObject hint in hints)
        {
            hint.SetActive(false);
        }
    }
}

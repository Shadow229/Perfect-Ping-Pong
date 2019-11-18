using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LoadingText : MonoBehaviour
{
    public string[] DisplayText;

    private TextMeshProUGUI TMPro;
    private int txtNo;

    // Start is called before the first frame update
    void Start()
    {
        //set up the loading text
        TMPro = GetComponent<TextMeshProUGUI>();
        txtNo = 0;
        StartCoroutine(UpdateLoadText());
    }

    //once unhidden loop itself and just display some loading messages so we know its still running
    IEnumerator UpdateLoadText()
    {
        TMPro.text = DisplayText[txtNo];

        txtNo = Random.Range(1, DisplayText.Length - 1);

        yield return new WaitForSeconds(1f);
        StartCoroutine(UpdateLoadText());
    }
}

using System.Collections;
using UnityEngine;
using TMPro;

//some fun loading texts if the game load takes longer than expected
public class LoadingText : MonoBehaviour
{
    [SerializeField]
    private string[] DisplayText = null;

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

    //once unhidden loop itself and just display any additional loading messages so we know its still running
    IEnumerator UpdateLoadText()
    {
        TMPro.text = DisplayText[txtNo];

        txtNo = Random.Range(1, DisplayText.Length - 1);

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(UpdateLoadText());
    }
}

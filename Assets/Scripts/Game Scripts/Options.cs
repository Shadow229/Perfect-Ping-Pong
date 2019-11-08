using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    public GameObject ScreenInfoDimensions;
    public Slider Sensitivity;
    public Slider MusicVol;


    private void Awake()
    {
        ScreenInfoDimensions.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.ScreenHeight + " x " + GameManager.Instance.ScreenWidth;
        Sensitivity.value = GameManager.Instance.SensitivityMultiplier;
        //initialise saved music values
        MusicVol.value = GameManager.Instance.MusicVol;
    }

    public void SetMusicVolume()
    {
        GameManager.Instance.SetMusicVolume(MusicVol.value);
    }

    public void ResetMusicVolume()
    {
        MusicVol.value = GameManager.Instance.MusicVol;
    }

    public void SetSensitiviy()
    {
        GameManager.Instance.SetSensitivity(Sensitivity.value);
    }

    public void ResetSensitivity()
    {
        Sensitivity.value = GameManager.Instance.SensitivityMultiplier;
    }

    public void PreviewMusicVolume()
    {
        //set it
        GameManager.Instance.GetComponent<AudioSource>().volume = MusicVol.value;
    }
}

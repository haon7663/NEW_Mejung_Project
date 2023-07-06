using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider MasterSlider;
    public Slider BgmSlider;
    public Slider SfxSlider;

    private void Start()
    {
        MasterSlider.value = PlayerPrefs.GetFloat("Master");
        BgmSlider.value = PlayerPrefs.GetFloat("BGM");
        SfxSlider.value = PlayerPrefs.GetFloat("SFX");
        SetMasterVolme();
        SetBgmVolme();
        SetSFXVolme();
    }
    private void OnDisable()
    {
        MasterSlider.value = PlayerPrefs.GetFloat("Master");
        BgmSlider.value = PlayerPrefs.GetFloat("BGM");
        SfxSlider.value = PlayerPrefs.GetFloat("SFX");
        SetMasterVolme();
        SetBgmVolme();
        SetSFXVolme();
    }

    public void SetMasterVolme()
    {
        float sound = MasterSlider.value;

        if (sound <= -40f) audioMixer.SetFloat("Master", -80);
        else audioMixer.SetFloat("Master", sound);

        PlayerPrefs.SetFloat("Master", sound);
    }

    public void SetBgmVolme()
    {
        float sound = BgmSlider.value;

        if (sound <= -40f) audioMixer.SetFloat("BGM", -80);
        else audioMixer.SetFloat("BGM", sound);

        PlayerPrefs.SetFloat("BGM", sound);
    }

    public void SetSFXVolme()
    {
        float sound = SfxSlider.value;

        if (sound <= -40f) audioMixer.SetFloat("SFX", -80);
        else audioMixer.SetFloat("SFX", sound);

        PlayerPrefs.SetFloat("SFX", sound);
    }
}

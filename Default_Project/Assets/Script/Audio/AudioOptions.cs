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

    public void SetMasterVolme()
    {
        float sound = MasterSlider.value;

        if (sound <= -40f) audioMixer.SetFloat("Master", -80);
        else audioMixer.SetFloat("Master", sound);
    }

    public void SetBgmVolme()
    {
        float sound = BgmSlider.value;

        if (sound <= -40f) audioMixer.SetFloat("BGM", -80);
        else audioMixer.SetFloat("BGM", sound);
    }

    public void SetSFXVolme()
    {
        float sound = SfxSlider.value;

        if (sound <= -40f) audioMixer.SetFloat("SFX", -80);
        else audioMixer.SetFloat("SFX", sound);
    }
}

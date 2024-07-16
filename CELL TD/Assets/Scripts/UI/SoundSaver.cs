using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundSaver : MonoBehaviour
{
    public Slider slider;
    public AudioMixer mixer;
    public string savedMixer;
    public string mixerName;
    // Start is called before the first frame update
    void Start()
    {
        SetVolume(PlayerPrefs.GetFloat(savedMixer, 100));
    }

    void SetVolume(float value)
    {
        if (value < 1)
        {
            value = .001f;
        }
        RefreshSlider(value);
        PlayerPrefs.GetFloat(savedMixer, value);
        if (mixer)
        {
            mixer.SetFloat(mixerName, Mathf.Log10(value / 100) * 20f);
        }
        else
        {
            //AudioListener.volume = Mathf.Log10(value / 100) * 20f;
        }
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(slider.value);
    }
    void RefreshSlider(float value)
    {
        slider.value = value;
    }
}
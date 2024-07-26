using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

using Assets.Scripts.UI.Settings;


public class SettingsWindow_AudioPanel : MonoBehaviour
{
    // The range of the volume slider on an AudioMixer
    // NOTE: The volume range on an AudioMixer is -80db to 20db.
    //       I made it a little smaller than that because it causes the music to be distorted when at max volume if the max is 20db.
    const float _MinVolume = -70f;
    const float _MaxVolume = 10f;


    [Header("UI Elements")]

    [SerializeField]
    private Slider _Slider_MasterVolume;
    [SerializeField]
    private Slider _Slider_SfxVolume;
    [SerializeField]
    private Slider _Slider_MusicVolume;


    [Header("Audio Mixing")]

    [SerializeField]
    private AudioMixerGroup _MasterAudioMixer;



    private void Awake()
    {
        InitUI();    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Initializes the UI based on the saved settings values in PlayerPrefs.
    /// If there is not an entry for a setting in PlayerPrefs, then the default value is used from AudioSettingsConstants.
    /// </summary>
    private void InitUI()
    {
        float value = PlayerPrefs.GetFloat(AudioSettingsConstants.MasterVolumeKey, 
                                           AudioSettingsConstants.Default_MasterVolume);
        _Slider_MasterVolume.value = Mathf.Clamp(value, 0, 100);


        value = PlayerPrefs.GetFloat(AudioSettingsConstants.SfxVolumeKey,
                                     AudioSettingsConstants.Default_SfxVolume);
        _Slider_SfxVolume.value = Mathf.Clamp(value, 0, 100);


        value = PlayerPrefs.GetFloat(AudioSettingsConstants.MusicVolumeKey,
                                     AudioSettingsConstants.Default_MusicVolume);
        _Slider_MusicVolume.value = Mathf.Clamp(value, 0, 100);
    }

    public void OnMasterVolumeChanged(float value)
    {
        // Update the volume level of the master AudioMixer.
        _MasterAudioMixer.audioMixer.SetFloat("Master", ConvertSliderValueToVolumeRange(value));

        // Save the new master volume level to PlayerPrefs.
        PlayerPrefs.SetFloat(AudioSettingsConstants.MasterVolumeKey, _Slider_MasterVolume.value);
    }

    public void OnSfxVolumeChanged(float value)
    {
        // Update the volume level of the SFX AudioMixer.
        _MasterAudioMixer.audioMixer.SetFloat("SFX", ConvertSliderValueToVolumeRange(value));

        // Save the new SFX volume level to PlayerPrefs.
        PlayerPrefs.SetFloat(AudioSettingsConstants.SfxVolumeKey, _Slider_SfxVolume.value);
    }

    public void OnMusicVolumeChanged(float value)
    {
        // Update the volume level of the music AudioMixer.
        _MasterAudioMixer.audioMixer.SetFloat("Music", ConvertSliderValueToVolumeRange(value));

        // Save the new music volume level to PlayerPrefs.
        PlayerPrefs.SetFloat(AudioSettingsConstants.MusicVolumeKey, _Slider_MusicVolume.value);
    }

    public static float ConvertSliderValueToVolumeRange(float sliderValue)
    {
        float percent = sliderValue / 100f;
        return _MinVolume + (percent * (_MaxVolume - _MinVolume));
    }

    public static void InitAudioVolumeLevels(AudioMixerGroup masterAudioMixer)
    {
        float value = PlayerPrefs.GetFloat(AudioSettingsConstants.MasterVolumeKey,
                                           AudioSettingsConstants.Default_MasterVolume);
        masterAudioMixer.audioMixer.SetFloat("Master", ConvertSliderValueToVolumeRange(Mathf.Clamp(value, 0, 100)));


        value = PlayerPrefs.GetFloat(AudioSettingsConstants.SfxVolumeKey,
                                     AudioSettingsConstants.Default_SfxVolume);
        masterAudioMixer.audioMixer.SetFloat("SFX", ConvertSliderValueToVolumeRange(Mathf.Clamp(value, 0, 100)));


        value = PlayerPrefs.GetFloat(AudioSettingsConstants.MusicVolumeKey,
                                     AudioSettingsConstants.Default_MusicVolume);
        masterAudioMixer.audioMixer.SetFloat("Music", ConvertSliderValueToVolumeRange(Mathf.Clamp(value, 0, 100)));
    }

}

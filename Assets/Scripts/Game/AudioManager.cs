using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class AudioManager : PersistentSingleton<AudioManager>
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        LoadOption();
    }

    void LoadOption()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0);
        MasterSoundVolume(masterVolume);
        masterSlider.value = masterVolume;

        float BGMVolume = PlayerPrefs.GetFloat("BGMVolume", 0);
        BGMSoundVolume(BGMVolume);
        bgmSlider.value = BGMVolume;

        float SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0);
        SFXSoundVolume(SFXVolume);
        sfxSlider.value = SFXVolume;
    }

    public void MasterSoundVolume(float value)
    {
        // 로그 스케일로 설정
        audioMixer.SetFloat("Master", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }
    public void BGMSoundVolume(float value)
    {
        audioMixer.SetFloat("BGSound",Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }
    public void SFXSoundVolume(float value)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }


}

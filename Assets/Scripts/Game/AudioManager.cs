using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : PersistentSingleton<AudioManager>
{
    public AudioMixer audioMixer;
    //public AudioSource bgmSound; // 일단 사용 안함

    public void MasterSoundVolume(float value)
    {
        //audioMixer.GetFloat("Master", out value);
        audioMixer.SetFloat("Master", Mathf.Log10(value) * 20);
    }
    public void BGMSoundVolume(float value)
    {
        audioMixer.SetFloat("BGSound",Mathf.Log10(value) * 20);
    }
    public void SFXSoundVolume(float value)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20);
    }

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound"); // 오브젝트 생성해서 소리 
        AudioSource audioSource = go.AddComponent<AudioSource>(); // 생성한 오브젝트에 AudioSource 추가 하기
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(audioSource, clip.length); // 다 재생되면 삭제
    }

    /*public void BGMSoundPlay(AudioClip clip)
    {
        bgmSound.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGSound")[0];
        bgmSound.clip = clip;
        bgmSound.loop = true;
        bgmSound.volume = 0.1f; // 이거는 다시 확인하기
        bgmSound.Play();
    }*/
}

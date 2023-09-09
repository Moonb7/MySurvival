using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : PersistentSingleton<AudioManager>
{
    public AudioMixer audioMixer;
    //public AudioSource bgmSound; // �ϴ� ��� ����

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
        GameObject go = new GameObject(sfxName + "Sound"); // ������Ʈ �����ؼ� �Ҹ� 
        AudioSource audioSource = go.AddComponent<AudioSource>(); // ������ ������Ʈ�� AudioSource �߰� �ϱ�
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(audioSource, clip.length); // �� ����Ǹ� ����
    }

    /*public void BGMSoundPlay(AudioClip clip)
    {
        bgmSound.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGSound")[0];
        bgmSound.clip = clip;
        bgmSound.loop = true;
        bgmSound.volume = 0.1f; // �̰Ŵ� �ٽ� Ȯ���ϱ�
        bgmSound.Play();
    }*/
}

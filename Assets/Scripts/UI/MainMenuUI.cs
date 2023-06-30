using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private string mainScene = "MainScene";

    [SerializeField]
    private SceneFader fader;

    // ���� ��ư
    public void OnStartButton()
    {
        fader.FadeTo(mainScene);
    }

    // ���� ��ư
    public void OnExitButton()
    {
        Application.Quit();
        Debug.Log("���� ����");
    }
}

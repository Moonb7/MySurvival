using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private string mainScene = "MainScene";

    [SerializeField]
    private SceneFader fader;

    // 시작 버튼
    public void OnStartButton()
    {
        fader.FadeTo(mainScene);
    }

    // 종료 버튼
    public void OnExitButton()
    {
        Application.Quit();
        Debug.Log("게임 종료");
    }
}

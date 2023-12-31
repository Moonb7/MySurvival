using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject upgradeUI;
    public GameObject creditsButton;

    public SceneFader fader;
    public string mainScene = "MainScene";
    public string creditsScene = "EndScene";

    public TextMeshProUGUI goldText;

    private void Start()
    {
        if (DataManager.Instance.playerData.isGameClear) // 게임 클리어 상태면
        {
            creditsButton.SetActive(true);
        }
    }

    private void Update()
    {
        goldText.text = DataManager.Instance.playerData.gold.ToString();
    }
    // 시작 버튼
    public void OnStartButton()
    {
        fader.FadeTo(mainScene);
    }

    public void OnUpgradeButton()
    {
        mainMenuUI.SetActive(false);
        OptionUI.Instance.optionButton.SetActive(false);

        upgradeUI.SetActive(true);
    }

    // 종료 버튼
    public void OnExitButton()
    {
        Application.Quit();
        DataManager.Instance.SaveData(); // 이거는 고려 게임이 종료버튼을 눌러서 저장하고 게임 실패시 저장하고 게임 클리어시 저장
        Debug.Log("게임 종료");
    }

    public void OnCreditsButton()
    {
        fader.FadeTo(creditsScene);
    }
}

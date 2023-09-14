using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject upgradeUI;

    public string mainScene = "MainScene";
    public SceneFader fader;

    public TextMeshProUGUI goldText;

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
}

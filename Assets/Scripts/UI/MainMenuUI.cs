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
    // ���� ��ư
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

    // ���� ��ư
    public void OnExitButton()
    {
        Application.Quit();
        DataManager.Instance.SaveData(); // �̰Ŵ� ��� ������ �����ư�� ������ �����ϰ� ���� ���н� �����ϰ� ���� Ŭ����� ����
        Debug.Log("���� ����");
    }
}

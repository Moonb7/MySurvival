using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject upgradeUI;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenceText;

    public TextMeshProUGUI[] buttonText;

    public Button[] upgradeButton;
    private int[] currentLevel = new int[3]; // 이거 저장해야됨
    public int[] maxLevel;

    private void Start()
    {
        for (int i = 0; i < currentLevel.Length; i++) // 저장 데이터 가져오기
        {
            currentLevel[i] = DataManager.Instance.playerData.upgradeLevel[i];
        }
        SetText();
    }

    public void SetText()
    {
        healthText.text = DataManager.Instance.playerData.maxHealth.ToString();
        attackText.text = DataManager.Instance.playerData.attack.ToString();
        defenceText.text = DataManager.Instance.playerData.defence.ToString();

        for (int i = 0; i < buttonText.Length; i++)
        {
            if (currentLevel[i] >= maxLevel[i])
            {
                upgradeButton[i].enabled = false;
            }
            buttonText[i].text = $"{GetUpgradePrice(currentLevel[i] + 1)}G \n{currentLevel[i]}/{maxLevel[i]}";
        }
    }

    public void BackButton()
    {
        upgradeUI.SetActive(false);

        OptionUI.Instance.optionButton.SetActive(true);
        mainMenuUI.SetActive(true);
    }

    public void UpgradeButton(int num)
    {
        switch (num)
        {
            case 0: // HP
                if(DataManager.Instance.UseGold(GetUpgradePrice(currentLevel[num] + 1)))
                {
                    DataManager.Instance.playerData.maxHealth += 100;
                    currentLevel[num]++;
                }
                break;
            case 1: // ATT
                if (DataManager.Instance.UseGold(GetUpgradePrice(currentLevel[num] + 1)))
                {
                    DataManager.Instance.playerData.attack += 3;
                    currentLevel[num]++;
                }
                break;
            case 2: // DEF
                if (DataManager.Instance.UseGold(GetUpgradePrice(currentLevel[num] + 1)))
                {
                    DataManager.Instance.playerData.defence += 2;
                    currentLevel[num]++;
                }
                break;
        }
        SetText();

        DataManager.Instance.playerData.upgradeLevel[num] = currentLevel[num]; // 데이터 저장
    }

    public int GetUpgradePrice(int nowLevel)
    {
        return nowLevel * 100; // 레벨업에 도달할 경험치 설정 1레벨에 100,2렙에 200 이런식으로 설정해 놨지만
        /*int prev = 0;
        int current = 100;  // 처음은 100부터 시작
        for (int i = 0; i < nowLevel; i++) // 피보나치 수열로 한번 해보았다.
        {
            nextExp = prev + current;
            prev = current;
            current = nextExp;
        }
        return nextExp;*/
    }
}

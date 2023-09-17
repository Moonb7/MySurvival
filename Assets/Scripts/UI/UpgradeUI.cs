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
    private int[] currentLevel = new int[3]; // �̰� �����ؾߵ�
    public int[] maxLevel;

    private void Start()
    {
        for (int i = 0; i < currentLevel.Length; i++) // ���� ������ ��������
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

        DataManager.Instance.playerData.upgradeLevel[num] = currentLevel[num]; // ������ ����
    }

    public int GetUpgradePrice(int nowLevel)
    {
        return nowLevel * 100; // �������� ������ ����ġ ���� 1������ 100,2���� 200 �̷������� ������ ������
        /*int prev = 0;
        int current = 100;  // ó���� 100���� ����
        for (int i = 0; i < nowLevel; i++) // �Ǻ���ġ ������ �ѹ� �غ��Ҵ�.
        {
            nextExp = prev + current;
            prev = current;
            current = nextExp;
        }
        return nextExp;*/
    }
}

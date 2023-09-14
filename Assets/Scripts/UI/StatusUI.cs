using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class StatusUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public Image expImage;
    public TextMeshProUGUI expText;

    private void Update()
    {
        SetGoldText();
        SetExpText();
    }

    public void SetGoldText()
    {
        goldText.text = DataManager.Instance.playerData.gold.ToString();
    }

    public void SetExpText()
    {
        double expfillAmount = Convert.ToDouble(PlayerStats.instance.exp) / Convert.ToDouble(PlayerStats.instance.GetLevelupExp(PlayerStats.instance.level)); // int���¶� ����ȯ�Ͽ� �������
        expImage.fillAmount = (float)expfillAmount;
        expText.text = $"{PlayerStats.instance.exp}   /   {PlayerStats.instance.GetLevelupExp(PlayerStats.instance.level)}";
    }
}

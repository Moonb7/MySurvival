using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public Image expImage;
    public TextMeshProUGUI expText;

    void Update()
    {
        goldText.text = PlayerStats.Instance.gold.ToString();

        double expfillAmount = Convert.ToDouble(PlayerStats.Instance.exp) / Convert.ToDouble(PlayerStats.Instance.GetLevelupExp(PlayerStats.Instance.level)); // ����ȯ �غ���?
        expImage.fillAmount = (float)expfillAmount;
        expText.text = $"{PlayerStats.Instance.exp}   /   {PlayerStats.Instance.GetLevelupExp(PlayerStats.Instance.level)}";
    }
}

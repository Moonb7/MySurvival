using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class StatusUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI goldText;
    public Image expImage;
    public TextMeshProUGUI expText;

    void Update()
    {
        goldText.text = playerStats.gold.ToString();

        double expfillAmount = Convert.ToDouble(playerStats.exp) / Convert.ToDouble(playerStats.GetLevelupExp(playerStats.level)); // 형변환 해볼까?
        expImage.fillAmount = (float)expfillAmount;
        expText.text = $"{playerStats.exp}   /   {playerStats.GetLevelupExp(playerStats.level)}";
    }
}

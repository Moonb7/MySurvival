using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI goldText;

    void Update()
    {
        goldText.text = playerStats.gold.ToString();
    }
}

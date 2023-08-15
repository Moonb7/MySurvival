using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public PlayerStats playerStats;
    private TextMeshProUGUI goldText;
    void Start()
    {
        goldText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = playerStats.gold.ToString();
    }
}

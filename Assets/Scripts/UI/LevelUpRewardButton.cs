using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelUpRewardButton : MonoBehaviour
{
    public LevelUpReward reward;
    public TextMeshProUGUI descriptText;
    public float value;

    public static UnityAction OnRewaedButton;

    private void Start()
    {
        SetDescriptText();
    }

    public void SetDescriptText()
    {
        switch (reward)
        {
            case LevelUpReward.Health:
                descriptText.text = $"최대 체력이 {value}만큼 증가합니다.";
                break;

            case LevelUpReward.Attack:
                descriptText.text = $"공격력이 {value}만큼 증가합니다.";
                break;

            case LevelUpReward.Defence:
                descriptText.text = $"방어력이 {value}만큼 증가합니다.";
                break;
        }
    }

    public void RewardButton()
    {
        switch (reward)
        {
            case LevelUpReward.Health:
                PlayerStats.instance.maxHealth.AddValue(value);
                break;

            case LevelUpReward.Attack:
                PlayerStats.instance.attack.AddValue(value);
                break;

            case LevelUpReward.Defence:
                PlayerStats.instance.defence.AddValue(value);
                break;
        }
        OnRewaedButton?.Invoke();
    }
}

public enum LevelUpReward
{
    Health,
    Attack,
    Defence
}
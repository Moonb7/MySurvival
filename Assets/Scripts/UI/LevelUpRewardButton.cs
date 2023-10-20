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
                descriptText.text = $"�ִ� ü���� {value}��ŭ �����մϴ�.";
                break;

            case LevelUpReward.Attack:
                descriptText.text = $"���ݷ��� {value}��ŭ �����մϴ�.";
                break;

            case LevelUpReward.Defence:
                descriptText.text = $"������ {value}��ŭ �����մϴ�.";
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
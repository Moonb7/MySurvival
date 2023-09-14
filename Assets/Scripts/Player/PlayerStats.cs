using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;

// ������ ������ �� Player ����
public class PlayerStats : CharacterStats
{
    public static PlayerStats instance; // �̱������� �����
    public int exp { get; private set; }
    public int level { get; private set; }

    public UnityAction OnLevelup; // �������ÿ� �����Ұ͵� �߰� �ϱ� ���� �����̳� ��Ÿ �̺�Ʈ�߰��ϱ�

    private void Awake()
    {
        #region �̱���
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        #endregion
    }

    protected override void Start()
    {
        maxHealth.SetValue( DataManager.Instance.playerData.maxHealth);
        attack.SetValue(DataManager.Instance.playerData.attack);
        defence.SetValue(DataManager.Instance.playerData.defence);
        level = 1;

        base.Start();
    }

    private void Update()
    {
        bool isLevelup = false;
        if (exp >= GetLevelupExp(level))
        {
            if(LevelUpUI.isReceived == false)
                return;

            // ������ ����
            OnLevelup?.Invoke();

            exp -= GetLevelupExp(level);
            level++;

            isLevelup = true;
        }
        if (isLevelup)
        {
            // ������ ȿ�� ����Ʈ�����ų�
        }
    }

    public void AddExp(int amount)
    {
        exp += amount;

        /*// ������ üũ
        bool isLevelup = false;
        while (exp >= GetLevelupExp(level))
        {
            *//*if (LevelUpUI.isReceived == false)
                continue;*//*

            // ������ ����
            OnLevelup?.Invoke();

            exp -= GetLevelupExp(level);
            level++;

            isLevelup = true;
        }
        if (isLevelup)
        {
            // ������ ȿ�� ����Ʈ�����ų�
        }*/

        Debug.Log($"Player�� ���� �ִ� ����ġ : {exp}");
    }
    public int GetLevelupExp(int nowLevel)
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

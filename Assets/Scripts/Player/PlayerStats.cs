using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ������ ������ �� Player ����
public class PlayerStats : PersistentSingleton<PlayerStats>
{
    public int gold { get; set; }
    public int exp { get; private set; }
    public int level { get; private set; }
    public int ammoCount { get; private set; } // �Ѿ� ����

    public static UnityAction OnLevelup; // �������ÿ� �����Ұ͵� �߰� �ϱ� ���� �����̳� ��Ÿ �̺�Ʈ�߰��ϱ�

    private void Start()
    {
        level = 1;
    }

    public void AddGold(int amount) // ��� ȹ��
    {
        gold += amount;
        Debug.Log($"Player�� ���� �ִ� ��差 : {gold}");
    }
    public bool UseGold(int amount) // ��� ���
    {
        if (gold < amount)
            return false;

        gold -= amount;
        return true;
    }

    public void AddExp(int amount)
    {
        exp += amount;

        // ������ üũ
        bool isLevelup = false;
        while (exp >= GetLevelupExp(level))
        {
            exp -= GetLevelupExp(level);
            level++;

            // ������ ����
            OnLevelup?.Invoke();

            isLevelup = true;
        }
        if (isLevelup)
        {
            // ������ ȿ�� ����Ʈ�����ų�
        }

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
    public void AddAmmor(int amount)
    {
        ammoCount += amount;
    }

    public bool UseAmmor(int amount)
    {
        if (ammoCount < amount)
            return false;

        ammoCount -= amount;
        return true;
    }
}

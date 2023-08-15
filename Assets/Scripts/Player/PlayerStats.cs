using System;
using UnityEngine;
using UnityEngine.Events;
using static Cinemachine.DocumentationSortingAttribute;

public class PlayerStats : CharacterStats 
{
    [SerializeField]
    private int startGold = 500;
    public int gold;
    public int exp;
    private int nextExp;            // ���� �������� ������ ����ġ ����
    public int level;

    public static UnityAction OnLevelup; // �������ÿ� �����Ұ͵� �߰� �ϱ� ���� �����̳� ��Ÿ �̺�Ʈ�߰��ϱ�

    protected override void Start()
    {
        base.Start();
        gold = startGold;
        level = 1;
    }

    public override void HitEffect()
    {
        base.HitEffect();    
        PlayerController.animator.SetBool(AnimString.Instance.isAttack, false);     // ��ų�� ������ �Ҷ������� true�����ذ� false�� ��ȯ���־� ������ ������.
        PlayerController.animator.SetInteger(AnimString.Instance.attackStats, -1);  // �÷��̾ ������ ���ϸ� ���� ���ΰ��� �ʱ�ȭ �����־���.
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

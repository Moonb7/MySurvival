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
    private int nextExp;            // 다음 레벨업에 도달할 경험치 설정
    public int level;

    public static UnityAction OnLevelup; // 레벨업시에 실행할것들 추가 하기 위해 보상이나 기타 이벤트추가하기

    protected override void Start()
    {
        base.Start();
        gold = startGold;
        level = 1;
    }

    public override void HitEffect()
    {
        base.HitEffect();    
        PlayerController.animator.SetBool(AnimString.Instance.isAttack, false);     // 스킬밑 공격을 할때마마다 true시켜준걸 false로 변환해주어 공격을 끝낸다.
        PlayerController.animator.SetInteger(AnimString.Instance.attackStats, -1);  // 플레이어가 공격을 당하면 공격 중인것을 초기화 시켜주었다.
    }

    public void AddGold(int amount) // 골드 획득
    {
        gold += amount;
        Debug.Log($"Player가 갖고 있는 골드량 : {gold}");
    }
    public bool UseGold(int amount) // 골드 사용
    {
        if (gold < amount)
            return false;

        gold -= amount;
        return true;
    }
    public void AddExp(int amount)
    {
        exp += amount;

        // 레벨업 체크
        bool isLevelup = false;
        while (exp >= GetLevelupExp(level))
        {
            exp -= GetLevelupExp(level);
            level++;

            // 레벨업 보상
            OnLevelup?.Invoke();

            isLevelup = true;
        }
        if (isLevelup)
        {
            // 레벨업 효과 이펙트같은거나
        }
    }
    public int GetLevelupExp(int nowLevel)  
    {
        return nowLevel * 100; // 레벨업에 도달할 경험치 설정 1레벨에 100,2렙에 200 이런식으로 설정해 놨지만
        /*int prev = 0;
        int current = 100;  // 처음은 100부터 시작
        for (int i = 0; i < nowLevel; i++) // 피보나치 수열로 한번 해보았다.
        {
            nextExp = prev + current;
            prev = current;
            current = nextExp;
        }
        return nextExp;*/
    }
}

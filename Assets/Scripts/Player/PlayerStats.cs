using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;

// 저장할 데이터 및 Player 스텟
public class PlayerStats : CharacterStats
{
    public static PlayerStats instance; // 싱글톤으로 만들기
    public int exp { get; private set; }
    public int level { get; private set; }

    public UnityAction OnLevelup; // 레벨업시에 실행할것들 추가 하기 위해 보상이나 기타 이벤트추가하기

    private void Awake()
    {
        #region 싱글톤
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

            // 레벨업 보상
            OnLevelup?.Invoke();

            exp -= GetLevelupExp(level);
            level++;

            isLevelup = true;
        }
        if (isLevelup)
        {
            // 레벨업 효과 이펙트같은거나
        }
    }

    public void AddExp(int amount)
    {
        exp += amount;

        /*// 레벨업 체크
        bool isLevelup = false;
        while (exp >= GetLevelupExp(level))
        {
            *//*if (LevelUpUI.isReceived == false)
                continue;*//*

            // 레벨업 보상
            OnLevelup?.Invoke();

            exp -= GetLevelupExp(level);
            level++;

            isLevelup = true;
        }
        if (isLevelup)
        {
            // 레벨업 효과 이펙트같은거나
        }*/

        Debug.Log($"Player가 갖고 있는 경험치 : {exp}");
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

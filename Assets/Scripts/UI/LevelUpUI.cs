using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUI : ItemUI
{
    public static bool isReceived = true; // 보상 받았는지 판단여부

    private void Start()
    {
        PlayerStats.instance.OnLevelup += OpenUI;
        LevelUpRewardButton.OnRewaedButton += CloseUI;
    }
    public override void OpenUI()
    {
        base.OpenUI();
        Time.timeScale= 0;
        isReceived = false;
    }

    public override void CloseUI()
    {
        base.CloseUI();

        //마우스 커서
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isReceived = true;
        Time.timeScale = 1;
    }

    private void OnDisable() // 삭제시에 이벤트 핸들러를 꼭 빼주자 에러가 발생할 수 있다
    {
        PlayerStats.instance.OnLevelup -= OpenUI;
        LevelUpRewardButton.OnRewaedButton -= CloseUI;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUI : ItemUI
{
    public static bool isReceived = true; // ���� �޾Ҵ��� �Ǵܿ���

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

        //���콺 Ŀ��
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isReceived = true;
        Time.timeScale = 1;
    }

    private void OnDisable() // �����ÿ� �̺�Ʈ �ڵ鷯�� �� ������ ������ �߻��� �� �ִ�
    {
        PlayerStats.instance.OnLevelup -= OpenUI;
        LevelUpRewardButton.OnRewaedButton -= CloseUI;
    }
}

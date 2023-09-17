using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearUI : ItemUI
{
    public SceneFader fader;
    public string endScene;

    public void OnCheckButton()
    {
        DataManager.Instance.playerData.isGameClear = true; // ���� Ŭ���� ���� 
        DataManager.Instance.SaveData(); // ������ ����

        fader.FadeTo(endScene); // ���� ũ�������� 
    }
}

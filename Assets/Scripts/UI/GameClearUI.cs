using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearUI : ItemUI
{
    public SceneFader fader;
    public string endScene;

    public void OnCheckButton()
    {
        DataManager.Instance.playerData.isGameClear = true; // 게임 클리어 저장 
        DataManager.Instance.SaveData(); // 데이터 저장

        fader.FadeTo(endScene); // 엔딩 크레딧으로 
    }
}

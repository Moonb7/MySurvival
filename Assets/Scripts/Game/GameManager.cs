using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI playTime;
    private float startTime;

    private void Start()
    {
        startTime = Time.time; // 게임 시작 시간 기록
    }

    private void Update()
    {
        SetPlayTime();
    }

    void SetPlayTime()
    {

        // 경과 시간 계산
        float elapsedTime = Time.time - startTime;

        // 분과 초 계산
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if(elapsedTime >= 31)
            return;
        // 시간 표시 형식으로 변환
        string timeString = minutes.ToString("00") + " : " + seconds.ToString("00");

        playTime.text = timeString;
    }
}

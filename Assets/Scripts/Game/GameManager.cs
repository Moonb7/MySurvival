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
        startTime = Time.time; // ���� ���� �ð� ���
    }

    private void Update()
    {
        SetPlayTime();
    }

    void SetPlayTime()
    {

        // ��� �ð� ���
        float elapsedTime = Time.time - startTime;

        // �а� �� ���
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if(elapsedTime >= 31)
            return;
        // �ð� ǥ�� �������� ��ȯ
        string timeString = minutes.ToString("00") + " : " + seconds.ToString("00");

        playTime.text = timeString;
    }
}

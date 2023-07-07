using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("여러번 실행하면서 간단하게 몇가지 시험하기 위해 만듬 나중에 이 코드는 지우든지 체크 필수")]
    public bool notSpawn; // 여러번 실행하면서 간단하게 몇가지 시험하기 위해 만듬 나중에 이 코드는 지우든지 체크 필수

    public TextMeshProUGUI playTime;
    private float startTime;
    private float elapsedTime; // 경과 시간 계산

    public GameObject player;
    public GameObject defaultEnemyPrefab;
    [Tooltip("Player주변 범위에 랜덤으로 생성 할 거기 때문에 범위를 고려하여 설정")]
    public float range;
    private Vector3 randomPos;
    private float spawnTime;
    private bool firstSpawn = false; // 초반 유니크 몬스터 생성때 사용

    private void Start()
    {
        startTime = Time.time; // 게임 시작 시간 기록
    }

    private void Update()
    {
        SetPlayTime();

        /*if (elapsedTime >= 180 && !firstSpawn) // 유니크 적 생성시 사용
        {
            SpawnEnemy();
            firstSpawn = true;
        }*/

        spawnTime += Time.deltaTime;
        if (spawnTime >= 10)
        {
            DefaultSpawnEnemy();
            spawnTime = 0;
        }
        
    }

    private void DefaultSpawnEnemy() //Enemy Spawn
    {
        if (notSpawn)
            return;

        for (int i = 0;i < 5; i++)
        {
            randomPos = new Vector3(
            Random.Range(player.transform.localPosition.x + range, player.transform.localPosition.x - range),
            defaultEnemyPrefab.transform.localPosition.y,
            Random.Range(player.transform.localPosition.z + range, player.transform.localPosition.z - range)
            );

            if(randomPos != player.transform.localPosition) // 플레이어 위치와 동일하지 않으면 
            Instantiate(defaultEnemyPrefab, randomPos, Quaternion.identity); // 적 생성
        }
    }

    void SetPlayTime()
    {
        // 경과 시간 계산
        elapsedTime = Time.time - startTime;

        // 분과 초 계산
        int minutes = Mathf.FloorToInt(elapsedTime / 60f); // 몫을 출력 하므로 분을 나타낼수 있다.
        int seconds = Mathf.FloorToInt(elapsedTime % 60f); // 나머지 출력를 출력하여 초를 나타 낼 수 있다.

        if(elapsedTime >= 60* 15 + 1)
            return;
        // 시간 표시 형식으로 변환
        string timeString = minutes.ToString("00") + " : " + seconds.ToString("00");

        playTime.text = timeString;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("������ �����ϸ鼭 �����ϰ� ��� �����ϱ� ���� ���� ���߿� �� �ڵ�� ������� üũ �ʼ�")]
    public bool notSpawn; // ������ �����ϸ鼭 �����ϰ� ��� �����ϱ� ���� ���� ���߿� �� �ڵ�� ������� üũ �ʼ�

    public TextMeshProUGUI playTime;
    private float startTime;
    private float elapsedTime; // ��� �ð� ���

    public GameObject player;
    public GameObject defaultEnemyPrefab;
    [Tooltip("Player�ֺ� ������ �������� ���� �� �ű� ������ ������ ����Ͽ� ����")]
    public float range;
    private Vector3 randomPos;
    private float spawnTime;
    private bool firstSpawn = false; // �ʹ� ����ũ ���� ������ ���

    private void Start()
    {
        startTime = Time.time; // ���� ���� �ð� ���
    }

    private void Update()
    {
        SetPlayTime();

        /*if (elapsedTime >= 180 && !firstSpawn) // ����ũ �� ������ ���
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

            if(randomPos != player.transform.localPosition) // �÷��̾� ��ġ�� �������� ������ 
            Instantiate(defaultEnemyPrefab, randomPos, Quaternion.identity); // �� ����
        }
    }

    void SetPlayTime()
    {
        // ��� �ð� ���
        elapsedTime = Time.time - startTime;

        // �а� �� ���
        int minutes = Mathf.FloorToInt(elapsedTime / 60f); // ���� ��� �ϹǷ� ���� ��Ÿ���� �ִ�.
        int seconds = Mathf.FloorToInt(elapsedTime % 60f); // ������ ��¸� ����Ͽ� �ʸ� ��Ÿ �� �� �ִ�.

        if(elapsedTime >= 60* 15 + 1)
            return;
        // �ð� ǥ�� �������� ��ȯ
        string timeString = minutes.ToString("00") + " : " + seconds.ToString("00");

        playTime.text = timeString;
    }
}

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
    public GameObject uniqueEnemyPrefab;
    public GameObject bossEnemyPrefab;
    [Tooltip("Player�ֺ� ������ �������� ���� �� �ű� ������ ������ ����Ͽ� ����")]
    public float range;
    private Vector3 randomPos;
    private float spawnTime;
    private bool UniqueSpawn = false; // �ʹ� ����ũ ���� ������ ���
    private bool BossSpawn = false; // �ʹ� ����ũ ���� ������ ���

    private void Start()
    {
        startTime = Time.time; // ���� ���� �ð� ���
    }

    private void Update()
    {
        OnPlayTime();

        if (notSpawn)
            return;

        spawnTime += Time.deltaTime;
        if (spawnTime >= 10)        // 10 �ʸ��� ���� �Ϲ� ���� �����ϱ� ����
        {
            DefaultSpawnEnemy();
            spawnTime = 0;
        }

        if (elapsedTime >= 180 && !UniqueSpawn) // ����ũ �� ������ ��� ��ȹ������ 3�п� ����ũ ���� ����
        {
            UniqueSpawnEnemy();
            UniqueSpawn = true;
        }

        if (elapsedTime >= 300 && !BossSpawn) // ����ũ �� ������ ��� ��ȹ������ 5�п� ���� ���� ����
        {
            BossSpawnEnemy();
            BossSpawn = true;
        }
    }

    private void DefaultSpawnEnemy() //Enemy Spawn
    {
        for (int i = 0;i < 5; i++)
        {
            randomPos = new Vector3(
            Random.Range(player.transform.localPosition.x + range, player.transform.localPosition.x - range),
            defaultEnemyPrefab.transform.localPosition.y,
            Random.Range(player.transform.localPosition.z + range, player.transform.localPosition.z - range)
            );

            if(randomPos != player.transform.localPosition) // �÷��̾� ��ġ�� ��ġ�� �ʰ� �ϱ�����
            Instantiate(defaultEnemyPrefab, randomPos, Quaternion.identity); // �� ����
        }
    }

    private void UniqueSpawnEnemy()
    {
        // ���⵵ Unique���� ���� ���� �������� �߰� ����

        randomPos = new Vector3(
            Random.Range(player.transform.localPosition.x + range, player.transform.localPosition.x - range),
            uniqueEnemyPrefab.transform.localPosition.y,
            Random.Range(player.transform.localPosition.z + range, player.transform.localPosition.z - range)
            );

        if (randomPos != player.transform.localPosition) // �÷��̾� ��ġ�� ��ġ�� �ʰ� �ϱ�����
            Instantiate(uniqueEnemyPrefab, randomPos, Quaternion.identity); // �� ����
    }

    private void BossSpawnEnemy()
    {
        // �Ϻ� ���� Ʈ���� �۵��ؼ� ���� �������� ���� �߰� �ϱ�

        randomPos = new Vector3(
            Random.Range(player.transform.localPosition.x + range, player.transform.localPosition.x - range),
            defaultEnemyPrefab.transform.localPosition.y,
            Random.Range(player.transform.localPosition.z + range, player.transform.localPosition.z - range)
            );

        if (randomPos != player.transform.localPosition) // �÷��̾� ��ġ�� ��ġ�� �ʰ� �ϱ�����
            Instantiate(bossEnemyPrefab, randomPos, Quaternion.identity); // �� ����
    }

    void OnPlayTime()
    {
        // ��� �ð� ���
        elapsedTime = Time.time - startTime;

        // �а� �� ���
        int minutes = Mathf.FloorToInt(elapsedTime / 60f); // ���� ��� �ϹǷ� ���� ��Ÿ���� �ִ�.
        int seconds = Mathf.FloorToInt(elapsedTime % 60f); // ������ ��¸� ����Ͽ� �ʸ� ��Ÿ �� �� �ִ�.

        if(elapsedTime >= 60* 15 + 1) // 15�оƻ��� �Ǹ� ���߰��ߴ�
            return;

        // �ð� ǥ�� �������� ��ȯ
        string timeString = minutes.ToString("00") + " : " + seconds.ToString("00");
        playTime.text = timeString;
    }
}

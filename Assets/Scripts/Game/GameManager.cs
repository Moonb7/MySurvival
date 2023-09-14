using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    public static bool notSpawn = false;
    public static AudioSource BGMaudio;

    public TextMeshProUGUI playTime;
    private float startTime;
    private float elapsedTime; // ��� �ð� ���
    private float cutSceneTime; // �ƽ� ��� �ð�

    public float[] spawnTime = new float[3];

    public GameObject player;
    public GameObject defaultEnemyPrefab;
    public GameObject uniqueEnemy;
    public GameObject bossEnemy;
    [Tooltip("Player�ֺ� ������ �������� ���� �� �ű� ������ ������ ����Ͽ� ����")]
    public float range;
    private Vector3 randomPos;
    private float countDown;
    private bool UniqueSpawn = false; // �ʹ� ����ũ ���� ������ ���
    private bool BossSpawn = false; // �ʹ� ����ũ ���� ������ ���
    private bool isCutScene = false;

    public PlayableDirector pd;
    public TimelineAsset[] timeLine;
    private void Start()
    {
        startTime = Time.time; // ���� ���� �ð� ���
        BGMaudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        OnPlayTime();

        if(Input.GetKey(KeyCode.H))
        {
            Time.timeScale = 30.0f;
        }
        if (Input.GetKey(KeyCode.J))
        {
            Time.timeScale = 1.0f;
        }
        if(Input.GetKey(KeyCode.V))
        {
            EnemyManager.Instance.ResumeEnemies();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            EnemyManager.Instance.PauseEnemies();
        }

        if (elapsedTime >= spawnTime[1] && !UniqueSpawn) // ����ũ �� ������ ��� ��ȹ������ 3�п� ����ũ ���� ����
        {
            UniqueSpawnEnemy();
            UniqueSpawn = true;
        }

        if (elapsedTime >= spawnTime[2] && !BossSpawn) // ����ũ �� ������ ��� ��ȹ������ 5�п� ���� ���� ����
        {
            if (isCutScene == false)
            {
                cutSceneTime = Time.time;
                OnCutsceneStart();
                isCutScene = true;
            }

            if (Time.time - cutSceneTime >= 12) // �ƽ��� �� 11�� ���� ���� 12�� ���ȸ� ����
            {
                EnemyManager.Instance.ResumeEnemies(); // ���� �ٽ� �����̱�
                BossSpawnEnemy();
                BossSpawn = true;
            }
        }
        

        if (notSpawn)
            return;
        countDown += Time.deltaTime;
        if (countDown >= spawnTime[0])        // 10 �ʸ��� ���� �Ϲ� ���� �����ϱ� ����
        {
            DefaultSpawnEnemy();
            countDown = 0;
        }
    }
    private void OnCutsceneStart()
    {
        pd.Play(timeLine[0]);
        EnemyManager.Instance.PauseEnemies();
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
            defaultEnemyPrefab.transform.localPosition.y,
            Random.Range(player.transform.localPosition.z + range, player.transform.localPosition.z - range)
            );

        if (randomPos != player.transform.localPosition) // �÷��̾� ��ġ�� ��ġ�� �ʰ� �ϱ�����
        {
            uniqueEnemy.SetActive(true);
            uniqueEnemy.transform.localPosition = randomPos;
            //Instantiate(uniqueEnemy, randomPos, Quaternion.identity); // �� ����
        }
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
        {
            bossEnemy.SetActive(true);
            bossEnemy.transform.localPosition = randomPos;
            //Instantiate(bossEnemyPrefab, randomPos, Quaternion.identity); // �� ����
        }
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

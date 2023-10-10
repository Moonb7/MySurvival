using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    public static AudioSource BGMaudio;
    public AudioClip gameClearSound;
    public AudioClip gameOverSound;

    public GameObject gameClearUI;
    public GameObject gameOverUI;
    public TextMeshProUGUI playTime;

    private float startTime;
    private float elapsedTime; // ��� �ð� ���
    private float bossCutSceneTime; // �ƽ� ��� �ð�
    [Tooltip("������ �̿�Ǵ� �������� �ƾ� �ð�")]
    public float bosscutTime;
    public float clearTime = 480f; // 8�б��� ��� ������ Ŭ�����ϰ� ����Ŵ�
    public float[] spawnTime = new float[3];

    public GameObject player;
    [Tooltip("�Ϲ� �� ������ ������ ����")]
    public int defaultEnemyCount = 3;
    public GameObject defaultEnemyPrefab;
    public GameObject uniqueEnemy;
    public GameObject bossEnemy;
    private BossEnemy bossEnemyStats;
    
    private float countDown;

    [Tooltip("Player�ֺ� ������ �������� ���� �� �ű� ������ ������ ����Ͽ� ����")]
    public float range;
    private Vector3 randomPos;

    public static bool notSpawn = false;
    private bool UniqueSpawn = false; // �ʹ� ����ũ ���� ������ ���
    private bool BossSpawn = false; // �ʹ� ����ũ ���� ������ ���
    private bool isCutScene = false;
    private bool isGameClear = false;
    private bool isGameOver = false;

    public PlayableDirector pd;
    public TimelineAsset[] timeLine;

    private void Start()
    {
        startTime = Time.time; // ���� ���� �ð� ���
        BGMaudio = GetComponent<AudioSource>();
        bossEnemyStats = bossEnemy.GetComponent<BossEnemy>();

        notSpawn = false; // static���� �Ѱ͵��� �ѹ��� �ʱ�ȭ ���ִ°� ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        #region ġƮŰ
        if (Input.GetKey(KeyCode.Alpha5))
        {
            Time.timeScale = 30.0f;
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            Time.timeScale = 0f;
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            Time.timeScale = 1.0f;
        }
        if (Input.GetKey(KeyCode.Alpha8))
        {
            EnemyManager.Instance.PauseEnemies();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            EnemyManager.Instance.ResumeEnemies();
        }
        #endregion

        if (elapsedTime >= spawnTime[1] && !UniqueSpawn) // ����ũ �� ������ ��� ��ȹ������ 3�п� ����ũ ���� ����
        {
            UniqueSpawnEnemy();
            UniqueSpawn = true;
        }

        if (elapsedTime >= spawnTime[2] && !BossSpawn) // ����ũ �� ������ ��� ��ȹ������ 5�п� ���� ���� ����
        {
            if (isCutScene == false)
            {
                bossCutSceneTime = Time.time;
                OnCutsceneStart();
                isCutScene = true;
            }

            if (Time.time - bossCutSceneTime >= bosscutTime) // �ƽ��� �� 11�� ���� ���� 12�� ���ȸ� ����
            {
                EnemyManager.Instance.ResumeEnemies(); // ���� �ٽ� �����̱�
                BossSpawnEnemy();
                BossSpawn = true;
            }
        }

        if (notSpawn)
            return;

        OnPlayTime();

        countDown += Time.deltaTime;
        if (countDown >= spawnTime[0])        // 10 �ʸ��� ���� �Ϲ� ���� �����ϱ� ����
        {
            DefaultSpawnEnemy();
            countDown = 0;
        }

        // Ŭ����
        if (elapsedTime >= clearTime || bossEnemyStats.isDeath && !isGameClear) // Ŭ���� Ÿ�ӱ��� ��� ���ų� ������ ������ Ŭ���� �Ѱɷ� ǥ��
        {
            GameClear();
        }

        // ���� ����
        if (PlayerStats.instance.isDeath && !isGameOver)
        {
            GameOver();
        }
    }

    private void GameClear()
    {
        BGMaudio.Stop();
        BGMaudio.loop = false;
        // vfx���� �߰�
        BGMaudio.clip = gameClearSound;
        BGMaudio.Play();

        gameClearUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        for (int i = 0; i < EnemyManager.Instance.enemies.Count; i++)
        {
            Destroy(EnemyManager.Instance.enemies[i].gameObject); // ���� �������ֱ�
        }
        EnemyManager.Instance.enemies.Clear(); // �������� �ʱ�ȭ �����ֱ�
        isGameClear = true;
        notSpawn = true; // ���̻� ������ �ȵǰ� �����
    }

    private void GameOver()
    {
        BGMaudio.Stop();
        BGMaudio.loop = false;
        // vfx���� �߰�
        BGMaudio.clip = gameOverSound;
        BGMaudio.Play();

        gameOverUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isGameOver = true;
        notSpawn = true; // ���̻� ������ �ȵǰ� �����
    }

    private void OnCutsceneStart()
    {
        pd.Play(timeLine[0]);
        EnemyManager.Instance.PauseEnemies();
    }

    private void DefaultSpawnEnemy() //Enemy Spawn
    {
        for (int i = 0;i < defaultEnemyCount; i++)
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
        if (BossSpawn)
        {
            elapsedTime = Time.time - startTime - bosscutTime;
        }
        else
        {
            elapsedTime = Time.time - startTime;
        }
        

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

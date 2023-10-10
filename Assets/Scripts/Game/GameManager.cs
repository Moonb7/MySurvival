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
    private float elapsedTime; // 경과 시간 계산
    private float bossCutSceneTime; // 컷신 경과 시간
    [Tooltip("실제로 이용되는 보스등장 컷씬 시간")]
    public float bosscutTime;
    public float clearTime = 480f; // 8분까지 살아 남으면 클리어하게 만들거다
    public float[] spawnTime = new float[3];

    public GameObject player;
    [Tooltip("일반 몹 스폰시 생성할 갯수")]
    public int defaultEnemyCount = 3;
    public GameObject defaultEnemyPrefab;
    public GameObject uniqueEnemy;
    public GameObject bossEnemy;
    private BossEnemy bossEnemyStats;
    
    private float countDown;

    [Tooltip("Player주변 범위에 랜덤으로 생성 할 거기 때문에 범위를 고려하여 설정")]
    public float range;
    private Vector3 randomPos;

    public static bool notSpawn = false;
    private bool UniqueSpawn = false; // 초반 유니크 몬스터 생성때 사용
    private bool BossSpawn = false; // 초반 유니크 몬스터 생성때 사용
    private bool isCutScene = false;
    private bool isGameClear = false;
    private bool isGameOver = false;

    public PlayableDirector pd;
    public TimelineAsset[] timeLine;

    private void Start()
    {
        startTime = Time.time; // 게임 시작 시간 기록
        BGMaudio = GetComponent<AudioSource>();
        bossEnemyStats = bossEnemy.GetComponent<BossEnemy>();

        notSpawn = false; // static으로 한것들은 한번씩 초기화 해주는게 좋다
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        #region 치트키
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

        if (elapsedTime >= spawnTime[1] && !UniqueSpawn) // 유니크 적 생성시 사용 기획에서의 3분에 유니크 몬스터 생성
        {
            UniqueSpawnEnemy();
            UniqueSpawn = true;
        }

        if (elapsedTime >= spawnTime[2] && !BossSpawn) // 유니크 적 생성시 사용 기획에서의 5분에 보스 몬스터 생성
        {
            if (isCutScene == false)
            {
                bossCutSceneTime = Time.time;
                OnCutsceneStart();
                isCutScene = true;
            }

            if (Time.time - bossCutSceneTime >= bosscutTime) // 컷신이 약 11초 정도 여서 12초 동안만 실행
            {
                EnemyManager.Instance.ResumeEnemies(); // 적들 다시 움직이기
                BossSpawnEnemy();
                BossSpawn = true;
            }
        }

        if (notSpawn)
            return;

        OnPlayTime();

        countDown += Time.deltaTime;
        if (countDown >= spawnTime[0])        // 10 초마다 생성 일반 몹을 생성하기 위해
        {
            DefaultSpawnEnemy();
            countDown = 0;
        }

        // 클리어
        if (elapsedTime >= clearTime || bossEnemyStats.isDeath && !isGameClear) // 클리어 타임까지 살아 남거나 보스를 잡으면 클리어 한걸로 표시
        {
            GameClear();
        }

        // 게임 실패
        if (PlayerStats.instance.isDeath && !isGameOver)
        {
            GameOver();
        }
    }

    private void GameClear()
    {
        BGMaudio.Stop();
        BGMaudio.loop = false;
        // vfx사운드 추가
        BGMaudio.clip = gameClearSound;
        BGMaudio.Play();

        gameClearUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        for (int i = 0; i < EnemyManager.Instance.enemies.Count; i++)
        {
            Destroy(EnemyManager.Instance.enemies[i].gameObject); // 전부 삭제해주기
        }
        EnemyManager.Instance.enemies.Clear(); // 마무리로 초기화 시켜주기
        isGameClear = true;
        notSpawn = true; // 더이상 스폰도 안되게 만들기
    }

    private void GameOver()
    {
        BGMaudio.Stop();
        BGMaudio.loop = false;
        // vfx사운드 추가
        BGMaudio.clip = gameOverSound;
        BGMaudio.Play();

        gameOverUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isGameOver = true;
        notSpawn = true; // 더이상 스폰도 안되게 만들기
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

            if(randomPos != player.transform.localPosition) // 플레이어 위치와 겹치지 않게 하기위해
            Instantiate(defaultEnemyPrefab, randomPos, Quaternion.identity); // 적 생성
        }
    }

    private void UniqueSpawnEnemy()
    {
        // 여기도 Unique몬스터 생성 연출 시작으로 추가 예정

        randomPos = new Vector3(
            Random.Range(player.transform.localPosition.x + range, player.transform.localPosition.x - range),
            defaultEnemyPrefab.transform.localPosition.y,
            Random.Range(player.transform.localPosition.z + range, player.transform.localPosition.z - range)
            );

        if (randomPos != player.transform.localPosition) // 플레이어 위치와 겹치지 않게 하기위해
        {
            uniqueEnemy.SetActive(true);
            uniqueEnemy.transform.localPosition = randomPos;
            //Instantiate(uniqueEnemy, randomPos, Quaternion.identity); // 적 생성
        }
    }

    private void BossSpawnEnemy()
    {
        // 일부 연출 트리거 작동해서 영상 시작으로 연출 추가 하기

        randomPos = new Vector3(
            Random.Range(player.transform.localPosition.x + range, player.transform.localPosition.x - range),
            defaultEnemyPrefab.transform.localPosition.y,
            Random.Range(player.transform.localPosition.z + range, player.transform.localPosition.z - range)
            );

        if (randomPos != player.transform.localPosition) // 플레이어 위치와 겹치지 않게 하기위해
        {
            bossEnemy.SetActive(true);
            bossEnemy.transform.localPosition = randomPos;
            //Instantiate(bossEnemyPrefab, randomPos, Quaternion.identity); // 적 생성
        }
    }

    void OnPlayTime()
    {
        // 경과 시간 계산
        if (BossSpawn)
        {
            elapsedTime = Time.time - startTime - bosscutTime;
        }
        else
        {
            elapsedTime = Time.time - startTime;
        }
        

        // 분과 초 계산
        int minutes = Mathf.FloorToInt(elapsedTime / 60f); // 몫을 출력 하므로 분을 나타낼수 있다.
        int seconds = Mathf.FloorToInt(elapsedTime % 60f); // 나머지 출력를 출력하여 초를 나타 낼 수 있다.

        if(elapsedTime >= 60* 15 + 1) // 15분아상이 되면 멈추게했다
            return;

        // 시간 표시 형식으로 변환
        string timeString = minutes.ToString("00") + " : " + seconds.ToString("00");
        playTime.text = timeString;
    }
}

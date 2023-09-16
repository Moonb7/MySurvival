using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public List<Enemy> enemies = new List<Enemy>();

    public void AddEnemy(Enemy enemy) // 생성된 적들 추가하기
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public void PauseEnemies() // 일시정지에 적들을 멈추게 하기위해
    {
        GameManager.notSpawn = true;
        GameManager.BGMaudio.Stop();
        foreach (Enemy enemy in enemies)
        {
            enemy.SetState(EnemyState.Idle);
            enemy.agent.speed= 0;
        }
    }

    public void ResumeEnemies() // 정지 풀릴시
    {
        GameManager.notSpawn = false;
        GameManager.BGMaudio.Play();
        foreach (Enemy enemy in enemies)
        {
            enemy.SetState(EnemyState.Chase);
            enemy.agent.speed = enemy.beforSpeed;
        }
    }
}

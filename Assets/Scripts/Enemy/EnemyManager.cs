using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public List<Enemy> enemies = new List<Enemy>();

    public void AddEnemy(Enemy enemy) // ������ ���� �߰��ϱ�
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public void PauseEnemies() // �Ͻ������� ������ ���߰� �ϱ�����
    {
        GameManager.notSpawn = true;
        GameManager.BGMaudio.Stop();
        foreach (Enemy enemy in enemies)
        {
            enemy.SetState(EnemyState.Idle);
            enemy.agent.speed= 0;
        }
    }

    public void ResumeEnemies() // ���� Ǯ����
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

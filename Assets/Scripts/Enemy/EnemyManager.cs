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

    public void PauseEnemies() // �Ͻ������� ������ ���߰� �ϱ�����
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.currentStats = EnemyState.Idle;
            enemy.agent.speed= 0;
        }
    }

    public void ResumeEnemies() // ���� Ǯ����
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.currentStats = EnemyState.Chase;
            enemy.agent.speed = enemy.beforSpeed;
        }
    }
}

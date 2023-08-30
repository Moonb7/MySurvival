using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
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
        }
    }

    public void ResumeEnemies() // ���� Ǯ����
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.currentStats = EnemyState.Chase;
        }
    }
}

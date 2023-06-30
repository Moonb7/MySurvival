using UnityEngine;

public class TestSpawn_enemies : Interactive
{
    public GameObject enemyPrefab;
    public float range;
    private Vector3 randomPos;

    private void Update()
    {
        randomPos = new Vector3(
            Random.Range(player.transform.localPosition.x + range, player.transform.localPosition.x - range),
            enemyPrefab.transform.localPosition.y,
            Random.Range(player.transform.localPosition.z + range, player.transform.localPosition.z - range)
            );
    }

    public override void DoAction()
    {
        Instantiate(enemyPrefab, randomPos, Quaternion.identity);
    }
}

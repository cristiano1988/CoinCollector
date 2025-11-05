using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnY = 12f;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            float wait = Random.Range(0.5f, 1f);
            yield return new WaitForSeconds(wait);

            Vector3 pos = new Vector3(Random.Range(GameBounds.minX, GameBounds.maxX), spawnY, 0);
            Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
    }
}
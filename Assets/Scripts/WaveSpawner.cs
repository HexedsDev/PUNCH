using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject enemyPrefab;
    public int enemiesPerWave = 8;
    public int maxEnemiesAlive = 20;
    public float timeBetweenWaves = 6f;
    public float timeBetweenSpawns = 0.5f;

    [Header("Banana Spawning")]
    public GameObject bananaPrefab;
    public int bananasPerWave = 3;
    public float spawnRadius = 14f;

    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        yield return new WaitForSeconds(1.5f);

        while (true)
        {
            currentWave++;
            if (GameUI.Instance != null) GameUI.Instance.SetWaveText(currentWave);

            yield return StartCoroutine(SpawnWave());
            SpawnBananas();

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator SpawnWave()
    {
        int toSpawn = enemiesPerWave + (currentWave - 1) * 2;
        int spawned = 0;

        while (spawned < toSpawn)
        {
            int alive = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (alive < maxEnemiesAlive)
            {
                Instantiate(enemyPrefab, GetSpawnPosition(), Quaternion.identity);
                spawned++;
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void SpawnBananas()
    {
        if (bananaPrefab == null) return;
        for (int i = 0; i < bananasPerWave; i++)
        {
            Vector2 pos = new Vector2(
                Random.Range(-18f, 18f),
                Random.Range(-10f, 10f));
            Instantiate(bananaPrefab, pos, Quaternion.identity);
        }
    }

    Vector2 GetSpawnPosition()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float r = spawnRadius + Random.Range(0f, 4f);
        return new Vector2(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r);
    }
}

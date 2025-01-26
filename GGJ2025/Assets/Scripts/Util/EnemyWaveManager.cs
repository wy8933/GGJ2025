using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Wave
{
    public string waveName;
    public int baseEnemyCount;
    public float spawnInterval;
    public EnemyType[] enemyTypes;
    public bool isBossWave;
    public EnemyType bossType;
    public bool isBuffWave;
}

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance;

    [Header("Wave Settings")]
    public List<Wave> waves;
    public int currentWaveIndex = 0;
    private bool _isWaveActive = false;

    [Header("Scaling Difficulty")]
    public float enemyIncreaseFactor = 1.2f;

    [Header("Wave Timing")]
    public float timeBetweenWaves = 5f;

    [Header("Spawner Settings")]
    public List<EnemySpawner> enemySpawners;

    private int _enemiesRemainingToSpawn;
    private int _enemiesRemainingAlive;

    public delegate void OnWaveStart(int waveIndex);
    public event OnWaveStart WaveStarted;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(StartWaveCoroutine());
    }

    private IEnumerator StartWaveCoroutine()
    {
        yield return new WaitForSeconds(3f);
        StartNextWave();
    }

    private void StartNextWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("All waves completed!");
            return;
        }

        Wave currentWave = waves[currentWaveIndex];

        int scaledEnemyCount = Mathf.RoundToInt(currentWave.baseEnemyCount * Mathf.Pow(enemyIncreaseFactor, currentWaveIndex));
        _enemiesRemainingToSpawn = scaledEnemyCount;
        _enemiesRemainingAlive = _enemiesRemainingToSpawn;

        _isWaveActive = true;

        WaveStarted?.Invoke(currentWaveIndex);
        StartCoroutine(SpawnWaveEnemies(currentWave, scaledEnemyCount));
    }

    private IEnumerator SpawnWaveEnemies(Wave wave, int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            if (enemySpawners.Count == 0)
            {
                Debug.LogWarning("No enemy spawners assigned.");
                yield break;
            }

            // Spawn from a random spawner
            EnemySpawner selectedSpawner = enemySpawners[Random.Range(0, enemySpawners.Count)];
            selectedSpawner.SpawnEnemy();

            yield return new WaitForSeconds(wave.spawnInterval);
        }

        _isWaveActive = false;
        StartCoroutine(WaitForNextWave());
    }

    private IEnumerator WaitForNextWave()
    {
        while (_enemiesRemainingAlive > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(timeBetweenWaves);
        currentWaveIndex++;
        StartNextWave();
    }

    public void EnemyDefeated()
    {
        _enemiesRemainingAlive--;
    }
}

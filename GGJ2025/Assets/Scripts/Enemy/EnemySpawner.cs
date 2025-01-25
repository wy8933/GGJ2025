using System.Runtime.CompilerServices;
using UnityEngine;
using ObjectPoolings;
using NUnit.Framework;
using System.Collections.Generic;
public enum EnemyType {
    germ1,
    germ2
}

public class EnemySpawner : MonoBehaviour
{
    public EnemyType enemyType = EnemyType.germ1;
    public List<GameObject> enemyPrefabs;

    [Header("Enemy Time Spawner")]
    public bool isTimeSpawner;
    public float spawnTime;
    float spawnTimer;

    private void Update()
    {
        if (isTimeSpawner) 
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer < 0)
            {
                SpawnEnemy();
                spawnTimer = spawnTime;
            }
        }
    }

    private void SpawnEnemy() {
        var (objectInstance, enemyPool) = ObjectPooling.GetOrCreate(enemyPrefabs[(int)enemyType], transform.position, transform.rotation, "Enemies");
        objectInstance.GetComponent<BaseEnemy>().InitEnemy(enemyPool);
    }

}

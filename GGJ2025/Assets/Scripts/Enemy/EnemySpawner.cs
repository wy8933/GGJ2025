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
    float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) 
        { 
            SpawnEnemy();
            timer = 5;
        }
    }


    private void SpawnEnemy() {
        var (objectInstance, enemyPool) = ObjectPooling.GetOrCreate(enemyPrefabs[(int)enemyType], transform.position, transform.rotation, "Enemies");
        objectInstance.GetComponent<BaseEnemy>().InitEnemy(enemyPool);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

// * hello nicolas

public class Spawner : MonoBehaviour
{
    // * Array of spawn points
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenSpawns = 5f;

    private float timeSinceLastSpawn;

    [SerializeField] private Enemy enemyPrefab;
    private IObjectPool<Enemy> enemyPool;

    private void Awake()
    {
        enemyPool = new ObjectPool<Enemy>(CreateEnemy);
    }

    // * using the ObjectPool to create a new enemy
    // * and set the pool to the enemy
    private Enemy CreateEnemy()
    {
        Enemy enemy = Instantiate(enemyPrefab);
        enemy.SetPool(enemyPool);
        return enemy;

    }
    // * using the ObjectPool to get an enemy
    // * and set the position of the enemy

    void Update()
    {
        if (Time.time > timeSinceLastSpawn)
        {
            enemyPool.Get();
            timeSinceLastSpawn = Time.time + timeBetweenSpawns;
        }
    }
}
// * In EnemyAIMelee.cs, the enemy is Released after getting killed
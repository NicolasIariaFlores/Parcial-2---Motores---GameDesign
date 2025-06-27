using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private int enemiesCount;
    [SerializeField] private float spawnCD;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Collider2D patrolArea;

    private int enemiesSpawned; 
    private int enemiesAlive;

    public static EnemiesSpawner Instance { get; private set; }


    private void Awake()
    {
        Instance = this; 
    }

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < enemiesCount)
        {
            Spawn();
            enemiesSpawned++;
            enemiesAlive++; 
            yield return new WaitForSeconds(spawnCD);
        }
    }

    private void Spawn()
    {
        if (enemies.Count == 0)
        {
            Debug.LogWarning("No hay enemigos en la lista del spawner.");
            return;
        }

        int index = UnityEngine.Random.Range(0, enemies.Count);
        GameObject enemigoSeleccionado = enemies[index];

        GameObject enemyInstance = Instantiate(enemigoSeleccionado, spawnPoint.position, Quaternion.identity);

        EnemyPatrol patrolScript = enemyInstance.GetComponent<EnemyPatrol>();
        if (patrolScript != null && patrolArea != null)
        {
            patrolScript.SetPatrolArea(patrolArea); 
        }

        EnemyHealth enemyScript = enemyInstance.GetComponent<EnemyHealth>();
        if (enemyScript != null)
        {
            enemyScript.onDeath += OnEnemyDeath; 
        }
    }

    private void OnEnemyDeath()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            Debug.Log("Todso muertos");
        }
    }

    public bool AllEnemiesDefeated()
    {
        return enemiesAlive <= 0 && enemiesSpawned == enemiesCount;
    }
}


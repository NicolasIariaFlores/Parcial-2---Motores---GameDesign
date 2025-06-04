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
    private int enemiesSpawned; 

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
        Instantiate(enemigoSeleccionado, transform.position, Quaternion.identity);
    }
}

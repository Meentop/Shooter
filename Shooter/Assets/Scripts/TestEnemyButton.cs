using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyButton : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform spawnPoint;

    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyButton : MonoBehaviour, ISelectableItem
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform spawnPoint;

    public SelectableItems ItemType => SelectableItems.TestButton;

    public void OnSelect(Player player)
    {
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}

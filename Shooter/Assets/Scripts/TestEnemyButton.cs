using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyButton : MonoBehaviour, ISelectableItem
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;

    public SelectableItems ItemType => SelectableItems.TestButton;

    public string Text => "E";

    public void OnSelect(Player player)
    {
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}

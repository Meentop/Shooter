using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoldProfitConfig", menuName = "ScriptableObjects/GoldProfitConfig")]
public class GoldProfitConfig : ScriptableObject
{
    [SerializeField] private EnemyPrice[] enemyPrice;
    [SerializeField] private int chestGold;

    public int GetEnemiesPrice(Agent agent)
    {
        foreach (EnemyPrice enemy in enemyPrice)
        {
            if (enemy.agent.GetComponentInChildren<Agent>().GetName() == agent.GetName())
                return enemy.price;
        }
        return Vector3Int.zero.y;
    }

    public int GetChestGold() => chestGold;
}

[System.Serializable]
public struct EnemyPrice
{
    public GameObject agent;
    public int price;
}

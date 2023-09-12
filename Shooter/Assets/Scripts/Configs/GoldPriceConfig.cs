using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoldPriceConfig", menuName = "ScriptableObjects/GoldPriceConfig")]
public class GoldPriceConfig : ScriptableObject
{
    public EnemyPrice[] enemyPrice;

    public int GetEnemiesPrice(Agent agent)
    {
        foreach (EnemyPrice enemy in enemyPrice)
        {
            if (enemy.agent.GetComponentInChildren<Agent>().GetName() == agent.GetName())
                return enemy.price;
        }
        return Vector3Int.zero.y;
    }
}

[System.Serializable]
public struct EnemyPrice
{
    public GameObject agent;
    public int price;
}

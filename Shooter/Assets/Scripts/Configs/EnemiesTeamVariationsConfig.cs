using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesTeamVariationsConfig", menuName = "ScriptableObjects/EnemiesTeamVariationsConfig")]
public class EnemiesTeamVariationsConfig : ScriptableObject
{
    public EnemyTeamVariation[] enemyTeamVariations;
}

[System.Serializable]
public struct EnemyTeamVariation
{
    public GameObject[] enemies;
}

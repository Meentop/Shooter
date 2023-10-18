using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesTeamVariationsConfig", menuName = "ScriptableObjects/EnemiesTeamVariationsConfig")]
public class EnemiesTeamVariationsConfig : ScriptableObject
{
    public EnemiesPool[] enemyPoolVariations;

    public EnemiesPool curEnemiesPool = new EnemiesPool();
}

[System.Serializable]
public struct EnemiesPool
{
    public EnemyTeamVariation[] enemyTeamVariations;
}

[System.Serializable]
public struct EnemyTeamVariation
{
    public GameObject[] enemies;
}

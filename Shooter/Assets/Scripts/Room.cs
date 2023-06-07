using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    [HideInInspector] public Vector2Int place;

    [SerializeField] private Vector2Int[] directions;

    [SerializeField] private GameObject[] doors;

    [SerializeField] private float[] newRoomYPositions;

    [SerializeField] private EnemyGroup[] enemyGroups;

    private List<Enemy> enemies = new List<Enemy>();

    

    private void Start()
    {
        if (enemyGroups.Length > 0)
        {
            foreach (var enemy in enemyGroups[UnityEngine.Random.Range(0, enemyGroups.Length)].enemies)
            {
                enemies.Add(Instantiate(enemy.enemyPrefab, transform.position + enemy.position, Quaternion.identity));
            }
        }
    }

    public void SetOpenDoor(Vector2Int direction)
    {
        doors[Array.IndexOf(directions, direction)].SetActive(false);
    }

    public Vector2Int[] GetDoorDirections()
    {
        return directions;
    }

    public bool HasDoorInDirection(Vector2Int direction)
    {
        foreach (var dir in directions)
        {
            if (dir.x == direction.x && dir.y == direction.y)
                return true;
        }
        return false;
    }

    public float GetYPosition(Vector2Int direction)
    {
        return newRoomYPositions[Array.IndexOf(directions, direction)];
    }

    public int GetEnemyCount()
    {
        return enemies.Count;
    }

    public void ActivateEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.enabled = true;
        }
    }

    [Serializable]
    struct EnemyGroup
    {
        public SpawnEnemy[] enemies;

        [Serializable]
        public struct SpawnEnemy 
        {
            public Enemy enemyPrefab;
            public Vector3 position;
        }
    }
}

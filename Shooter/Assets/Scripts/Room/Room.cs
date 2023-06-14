using System;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [HideInInspector] public Vector2Int place;
    [SerializeField] private Vector2Int[] directions;
    [SerializeField] private GameObject[] doors;
    [SerializeField] private float[] newRoomYPositions;
    [SerializeField] private EnemyGroup[] enemyGroups;
    private List<Enemy> enemies = new List<Enemy>();

    private List<GameObject> activeDoors = new List<GameObject>();

    public void SpawnEnemies()
    {
        if (enemyGroups.Length > 0)
        {
            foreach (var enemy in enemyGroups[UnityEngine.Random.Range(0, enemyGroups.Length)].enemies)
            {
                Enemy enemy1 = Instantiate(enemy.enemyPrefab, transform.position + enemy.position, Quaternion.identity);
                enemies.Add(enemy1);
                enemy1.GetComponent<RemoveFromRoomAction>().room = this;
            }
        }
    }

    public void SetOpenDoor(Vector2Int direction)
    {
        int index = Array.IndexOf(directions, direction);
        doors[index].SetActive(false);
        activeDoors.Add(doors[index]);    
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

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        if(enemies.Count == 0)
        {
            SetDoors(false);
        }
    }

    public void SetDoors(bool value)
    {
        foreach (var door in activeDoors)
        {
            door.SetActive(value);
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
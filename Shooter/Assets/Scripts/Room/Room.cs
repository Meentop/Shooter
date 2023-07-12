using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private readonly Vector2Int[] _directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    [SerializeField] private Vector2Int[] directions;
    [SerializeField] private GameObject[] doors;
    [SerializeField] private float[] newRoomYPositions;
    [SerializeField] private EnemyGroup[] enemyGroups;
    [SerializeField] private bool isBattleRoom;
    [SerializeField] private AwardType awardType;
    [SerializeField] private Transform awardSpawnPoint;
    [SerializeField] private RoomAwardsConfig awardConfig;

    public float Height { get; private set; }

    private MapMiniController _miniController;
    private List<Enemy> enemies = new List<Enemy>();
    private List<GameObject> activeDoors = new List<GameObject>();

    public Dictionary<Vector2Int, Room> Neighours;

    public void Init(Vector2Int roomPos, float roomHeight, MapMiniController miniController, AwardType awardType)
    {
        Height = roomHeight;
        _miniController = miniController;
        Neighours = new Dictionary<Vector2Int, Room>();
        this.awardType = awardType;
        foreach (var direction in _directions)
        {
            Neighours.Add(roomPos + direction, null);
        }
    }

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

    public float GetDoorHeight(Vector2Int direction)
    {
        return newRoomYPositions[Array.IndexOf(directions, direction)];
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            SetDoors(false);
            SpawnAward(awardType);
        }
    }

    public void SetDoors(bool value)
    {
        foreach (var door in activeDoors)
        {
            door.SetActive(value);
        }
    }

    public void UpdateMiniMapPos(float moveSpeed)
    {
        StartCoroutine(MoveMiniMap());
        IEnumerator MoveMiniMap()
        {
            float elapsedTime = 0f;
            while(elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * moveSpeed;
                Vector3 startPos = _miniController.MiniRoomsHolder.localPosition;
                _miniController.MiniRoomsHolder.localPosition = Vector3.Lerp(startPos, new Vector3(-gameObject.transform.position.x, -gameObject.transform.position.z, 0) / 1.2f, elapsedTime);
                yield return null;
            } 
        }
    }
    
    public void SetActiveMiniRoom(bool isActive)
    {
        _miniController.SetActiveMiniRoom(isActive, GetComponent<Room>());
    }

    public bool IsBattleRoom()
    {
        return isBattleRoom;
    }

    public void SpawnAward(AwardType award)
    {
        if(awardSpawnPoint.childCount > 0)
            Destroy(awardSpawnPoint.GetChild(0).gameObject);
        if (awardConfig.awards[(int)award] != null)
            Instantiate(awardConfig.awards[(int)award], awardSpawnPoint);
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


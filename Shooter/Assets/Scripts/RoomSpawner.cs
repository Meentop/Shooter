using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private int numberOfRooms;
    [SerializeField] private Vector2 roomSize;
    [SerializeField] private Room startRoom;
    [SerializeField] private Room[] roomPrefabs;

    private Dictionary<Vector2Int, Room> _roomMap;

    public void Init()
    {
        Room startRoom = Instantiate(this.startRoom.gameObject, Vector3.zero, Quaternion.identity).GetComponent<Room>();
        startRoom.Init(Vector2Int.zero);
        _roomMap = new Dictionary<Vector2Int, Room>() { { Vector2Int.zero, startRoom } };

        for (int i = 0; i < numberOfRooms - 1; i++)
        {
                var roomsWithEmptyNaighbours = _roomMap.Where(room => room.Value.Naighours.Any(naighour => naighour.Value == null));
                var parentRoom = roomsWithEmptyNaighbours.ElementAt(Random.Range(0, roomsWithEmptyNaighbours.Count()));
                var freePlaces = parentRoom.Value.Naighours.Where(naighour => naighour.Value == null);
                var newRoomSpawnPos = freePlaces.ElementAt(Random.Range(0, freePlaces.Count()));
                var spawnedRoom = SpawnRoom(parentRoom, newRoomSpawnPos);
                spawnedRoom.Init(newRoomSpawnPos.Key);
                UpdateRoomMap(spawnedRoom, newRoomSpawnPos.Key);
        }
    }
    public static readonly Vector2Int[] directions = {Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left};

    private Room SpawnRoom(KeyValuePair<Vector2Int, Room> parentRoom, KeyValuePair<Vector2Int, Room> _newRoom)
    {
        Vector2Int direction = _newRoom.Key - parentRoom.Key;
        Vector3 flatPosition = new Vector3(direction.x, 0, direction.y) * roomSize.x + parentRoom.Value.transform.position;

        var suitablePrefabs = roomPrefabs.Where(roomPrefab => roomPrefab.HasDoorInDirection(parentRoom.Key - _newRoom.Key));
        var randomRoomPrefab = suitablePrefabs.ElementAt(Random.Range(0, suitablePrefabs.Count()));

        float yPosition = parentRoom.Value.GetYPosition(direction) + -randomRoomPrefab.GetYPosition(-direction);
        Vector3 position = new Vector3(flatPosition.x, flatPosition.y + yPosition, flatPosition.z);

        Room newRoom = Instantiate(randomRoomPrefab, position, Quaternion.identity);
        newRoom.place = parentRoom.Value.place + direction;
        newRoom.SetOpenDoor(parentRoom.Value.place - newRoom.place);
        parentRoom.Value.SetOpenDoor(newRoom.place - parentRoom.Value.place);
        return newRoom;
    }

    private void UpdateRoomMap(Room spawnedRoom, Vector2Int spawnedRoomPos)
    {
        for (int spawnedRoomNaighbourIndex = 0; spawnedRoomNaighbourIndex < spawnedRoom.Naighours.Count; spawnedRoomNaighbourIndex++)
        {
            var naighbour = spawnedRoom.Naighours.ElementAt(spawnedRoomNaighbourIndex);
            if (_roomMap.ContainsKey(naighbour.Key))
            {
                spawnedRoom.Naighours[naighbour.Key] = _roomMap[naighbour.Key];
                _roomMap[naighbour.Key].Naighours[spawnedRoomPos] = spawnedRoom;
            }
        }
        _roomMap.Add(spawnedRoomPos, spawnedRoom);
    }
}

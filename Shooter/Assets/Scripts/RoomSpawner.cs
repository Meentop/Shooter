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
            var roomsWithEmptyNaighbours = _roomMap.Where(room => room.Value.Naighours.Any(naighour => IsFreePlace(room, naighour)));
            var parentRoom = roomsWithEmptyNaighbours.ElementAt(Random.Range(0, roomsWithEmptyNaighbours.Count()));
            var freePlaces = parentRoom.Value.Naighours.Where(naighour => IsFreePlace(parentRoom, naighour));
            var newRoomSpawnPos = freePlaces.ElementAt(Random.Range(0, freePlaces.Count()));
            var spawnedRoom = SpawnRoom(parentRoom, newRoomSpawnPos);
            spawnedRoom.Init(newRoomSpawnPos.Key);
            UpdateRoomMap(spawnedRoom, newRoomSpawnPos.Key);
        }
    }

    private bool IsFreePlace(KeyValuePair<Vector2Int, Room> room, KeyValuePair<Vector2Int, Room> naighbour)
    {
        return room.Value.HasDoorInDirection(naighbour.Key - room.Key) && naighbour.Value == null;
    }

    private Room SpawnRoom(KeyValuePair<Vector2Int, Room> parentRoom, KeyValuePair<Vector2Int, Room> _newRoom)
    {
        var directionFromParent = _newRoom.Key - parentRoom.Key;
        var directionFromNewRoom = parentRoom.Key - _newRoom.Key;

        var suitablePrefabs = roomPrefabs.Where(roomPrefab => roomPrefab.HasDoorInDirection(directionFromNewRoom));
        var randomRoomPrefab = suitablePrefabs.ElementAt(Random.Range(0, suitablePrefabs.Count()));

        var roomHeight = parentRoom.Value.place.y + parentRoom.Value.GetDoorHeight(directionFromParent) + randomRoomPrefab.GetDoorHeight(directionFromNewRoom);
        var spawnPosition = new Vector3(_newRoom.Key.x, 0, _newRoom.Key.y) * roomSize.x + new Vector3(0, roomHeight, 0);

        Room newRoom = Instantiate(randomRoomPrefab, spawnPosition, Quaternion.identity);
        newRoom.place = _newRoom.Key;
        newRoom.SetOpenDoor(directionFromNewRoom);
        parentRoom.Value.SetOpenDoor(directionFromParent);

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

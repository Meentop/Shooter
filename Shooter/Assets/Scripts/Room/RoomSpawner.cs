using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private int numberOfRooms;
    [SerializeField] private Vector2 roomSize;
    [SerializeField] private Room startRoom, portalRoom;
    [SerializeField] private Room[] battleRoomPrefabs;
    [SerializeField] private MapMiniController mapMiniController;

    private Dictionary<Vector2Int, Room> _roomMap;

    public void Init()
    {
        Room startRoom = Instantiate(this.startRoom.gameObject, Vector3.zero, Quaternion.identity).GetComponent<Room>();
        mapMiniController.SpawnMiniStartRoom(startRoom);
        startRoom.Init(Vector2Int.zero, 0, mapMiniController);
        _roomMap = new Dictionary<Vector2Int, Room>() { { Vector2Int.zero, startRoom } };

        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            var roomsWithEmptyNaighbours = _roomMap.Where(room => room.Value.Neighours.Any(naighour => IsFreePlace(room, naighour)));
            var parentRoom = roomsWithEmptyNaighbours.ElementAt(Random.Range(0, roomsWithEmptyNaighbours.Count()));
            var freePlaces = parentRoom.Value.Neighours.Where(naighour => IsFreePlace(parentRoom, naighour));
            var newRoomSpawnPos = freePlaces.ElementAt(Random.Range(0, freePlaces.Count()));
            var spawnedRoom = SpawnRoom(parentRoom, newRoomSpawnPos, i);
            UpdateRoomMap(spawnedRoom, newRoomSpawnPos.Key);
            mapMiniController.SpawnMiniMap(spawnedRoom.transform.position / 1.2f, parentRoom.Key - newRoomSpawnPos.Key, new KeyValuePair<Vector2Int, Room>(newRoomSpawnPos.Key, spawnedRoom), parentRoom);
        }
    }

    private bool IsFreePlace(KeyValuePair<Vector2Int, Room> room, KeyValuePair<Vector2Int, Room> naighbour)
    {
        return room.Value.HasDoorInDirection(naighbour.Key - room.Key) && naighbour.Value == null;
    }

    private Room SpawnRoom(KeyValuePair<Vector2Int, Room> parentRoom, KeyValuePair<Vector2Int, Room> _newRoom, int iteration)
    {
        var directionFromParent = _newRoom.Key - parentRoom.Key;
        var directionFromNewRoom = parentRoom.Key - _newRoom.Key;
        
        var randomRoomPrefab = DefineRandomRoom(directionFromNewRoom, iteration);
        var roomHeight = parentRoom.Value.Height + parentRoom.Value.GetDoorHeight(directionFromParent) - randomRoomPrefab.GetDoorHeight(directionFromNewRoom);
        var spawnPosition = new Vector3(_newRoom.Key.x, 0, _newRoom.Key.y) * roomSize.x + new Vector3(0, roomHeight, 0);

        Room newRoom = Instantiate(randomRoomPrefab, spawnPosition, Quaternion.identity);
        newRoom.SetOpenDoor(directionFromNewRoom);
        newRoom.Init(_newRoom.Key, newRoom.transform.position.y, mapMiniController);

        parentRoom.Value.SetOpenDoor(directionFromParent);
        
        return newRoom;
    }

    private Room DefineRandomRoom(Vector2Int directionFromNewRoom, int iteration)
    {
        if (iteration == numberOfRooms - 2)
            return portalRoom;
        var suitablePrefabs = battleRoomPrefabs.Where(roomPrefab => roomPrefab.HasDoorInDirection(directionFromNewRoom));
        return suitablePrefabs.ElementAt(Random.Range(0, suitablePrefabs.Count()));
    }

    private void UpdateRoomMap(Room spawnedRoom, Vector2Int spawnedRoomPos)
    {
        for (int spawnedRoomNaighbourIndex = 0; spawnedRoomNaighbourIndex < spawnedRoom.Neighours.Count; spawnedRoomNaighbourIndex++)
        {
            var naighbour = spawnedRoom.Neighours.ElementAt(spawnedRoomNaighbourIndex);
            if (_roomMap.ContainsKey(naighbour.Key))
            {
                spawnedRoom.Neighours[naighbour.Key] = _roomMap[naighbour.Key];
                _roomMap[naighbour.Key].Neighours[spawnedRoomPos] = spawnedRoom;
            }
        }
        _roomMap.Add(spawnedRoomPos, spawnedRoom);
    }
}

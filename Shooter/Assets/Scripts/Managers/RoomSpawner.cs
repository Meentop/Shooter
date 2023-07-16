using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private MapMiniController mapMiniController;

    private Dictionary<Vector2Int, Room> _roomMap;
    private RoomSpawnerInfo _roomSpawnerInfo;

    public void SetRoomSpawnerInfo(RoomSpawnerInfo roomSpawnerInfo)
    {
        _roomSpawnerInfo = roomSpawnerInfo;
    }
    
    public void Init()
    {
        Room startRoom = Instantiate(_roomSpawnerInfo.StartRoom.gameObject, Vector3.zero, Quaternion.identity).GetComponent<Room>();
        mapMiniController.SpawnMiniStartRoom(startRoom);
        startRoom.Init(Vector2Int.zero, 0, mapMiniController, AwardType.None);
        _roomMap = new Dictionary<Vector2Int, Room>() { { Vector2Int.zero, startRoom } };

        for (int i = 0; i < _roomSpawnerInfo.NumberOfRooms - 1; i++)
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
        var spawnPosition = new Vector3(_newRoom.Key.x, 0, _newRoom.Key.y) * _roomSpawnerInfo.RoomSize.x + new Vector3(0, roomHeight, 0);

        Room newRoom = Instantiate(randomRoomPrefab, spawnPosition, Quaternion.identity);
        newRoom.SetOpenDoor(directionFromNewRoom);
        AwardType randomAwardType = randomRoomPrefab.IsBattleRoom() ? (AwardType)Random.Range(1, (int)AwardType.ActiveSkill) : AwardType.None;
        newRoom.Init(_newRoom.Key, newRoom.transform.position.y, mapMiniController, randomAwardType);

        parentRoom.Value.SetOpenDoor(directionFromParent);
        
        return newRoom;
    }

    private Room DefineRandomRoom(Vector2Int directionFromNewRoom, int iteration)
    {
        if (iteration == _roomSpawnerInfo.NumberOfRooms - 2)
            return _roomSpawnerInfo.EndRoom;
        var suitablePrefabs = _roomSpawnerInfo.BattleRoomPrefabs.Where(roomPrefab => roomPrefab.HasDoorInDirection(directionFromNewRoom));
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

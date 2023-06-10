using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private int numberOfRooms;
    [SerializeField] private Vector2 roomSize;
    [SerializeField] private Room startRoom;
    [SerializeField] private Room[] roomPrefabs;

    private Room[,] _roomsMap;
    private List<Room> _rooms = new List<Room>();

    private void Start()
    {
        _roomsMap = new Room[(numberOfRooms * 2) - 1, (numberOfRooms * 2) - 1];
        Room startRoom = Instantiate(this.startRoom.gameObject, Vector3.zero, Quaternion.identity).GetComponent<Room>();
        _roomsMap[numberOfRooms - 1, numberOfRooms - 1] = startRoom;
        _rooms.Add(startRoom);
        startRoom.place = new Vector2Int(numberOfRooms - 1, numberOfRooms - 1);

        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            Room connectedRoom;
            Vector2Int direction;
            Room room;
            // check if new room place is free
            do
            {
                connectedRoom = _rooms[Random.Range(0, _rooms.Count)];
                direction = connectedRoom.GetDoorDirections()[Random.Range(0, connectedRoom.GetDoorDirections().Length)];
                room = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
            } while (_roomsMap[connectedRoom.place.x + direction.x, connectedRoom.place.y + direction.y] != null || !room.HasDoorInDirection(connectedRoom.place - (connectedRoom.place + direction)));
            SpawnRoom(connectedRoom, direction, room);
        }
    }
    public static readonly Vector2Int[] directions = {Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left};

    private void SpawnRoom(Room connectedRoom, Vector2Int direction, Room room)
    {
        Vector3 flatPosition = new Vector3(direction.x, 0, direction.y) * roomSize.x + connectedRoom.transform.position;    
        Vector2Int place = connectedRoom.place + direction;
        float yPosition = connectedRoom.GetYPosition(direction) + -room.GetYPosition(connectedRoom.place - place);
        Vector3 position = new Vector3(flatPosition.x, flatPosition.y + yPosition, flatPosition.z);
        Room newRoom = Instantiate(room, position, Quaternion.identity);
        _roomsMap[place.x, place.y] = newRoom;
        _rooms.Add(newRoom);
        newRoom.place = place;
        connectedRoom.SetOpenDoor(newRoom.place - connectedRoom.place);
        newRoom.SetOpenDoor(connectedRoom.place - newRoom.place);
    }
}

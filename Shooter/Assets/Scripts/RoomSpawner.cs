using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private int numberOfRooms;

    [SerializeField] private Vector2 roomSize;

    [SerializeField] private Room startRoom;

    [SerializeField] private Room[] roomPrefabs;

    private Room[,] roomsMap;
    private List<Room> rooms = new List<Room>();

    private void Start()
    {
        roomsMap = new Room[(numberOfRooms * 2) - 1, (numberOfRooms * 2) - 1];
        Room startRoom = Instantiate(this.startRoom.gameObject, Vector3.zero, Quaternion.identity).GetComponent<Room>();
        roomsMap[numberOfRooms - 1, numberOfRooms - 1] = startRoom;
        rooms.Add(startRoom);
        startRoom.place = new Vector2Int(numberOfRooms - 1, numberOfRooms - 1);

        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            Room connectedRoom;
            Vector2Int newRoomPlace;
            // check if new room place is free
            do
            {
                connectedRoom = rooms[Random.Range(0, rooms.Count)];
                newRoomPlace = connectedRoom.place + directions[Random.Range(0, directions.Length)];
            } while (roomsMap[newRoomPlace.x, newRoomPlace.y] != null);
            SpawnRoom(connectedRoom, newRoomPlace);
        }
    }
    Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };

    private void SpawnRoom(Room connectedRoom, Vector2Int place)
    {
        Vector3 direction = new Vector3(connectedRoom.place.x - place.x, 0, connectedRoom.place.y - place.y);
        Vector3 position = direction * (roomSize.x + 1) + connectedRoom.transform.position;
        Room room = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], position, Quaternion.identity);
        roomsMap[place.x, place.y] = room;
        rooms.Add(room);
        room.place = place;
        connectedRoom.SetOpenDoor(connectedRoom.place - room.place);
        room.SetOpenDoor(room.place - connectedRoom.place);
    }
}

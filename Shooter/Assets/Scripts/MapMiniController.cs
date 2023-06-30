using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMiniController : MonoBehaviour
{
    [SerializeField] private GameObject miniRoomPrefab;
    [SerializeField] private Transform miniRoomsHolder;
    [SerializeField] private Transform mapMiniRotateHolder;
    [SerializeField] private Transform playerTransform;
    public Transform MiniRoomsHolder { get { return miniRoomsHolder; } private set { miniRoomsHolder = value; } }

    private Dictionary<Room, MiniRoom> _miniRooms;

    private void Update()
    {
        mapMiniRotateHolder.localRotation = Quaternion.Euler(new Vector3(0, 0, playerTransform.localRotation.eulerAngles.y));
    }

    public void SpawnMiniMap(Vector3 spawnPosition, Vector2Int directionFromNewRoom, Room parentRoom)
    {
        MiniRoom newMinimapRoom = Instantiate(miniRoomPrefab, miniRoomsHolder).GetComponent<MiniRoom>();
        newMinimapRoom.transform.localPosition = new Vector3(spawnPosition.x, spawnPosition.z, 0);
        newMinimapRoom.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        _miniRooms.Add(parentRoom, newMinimapRoom);
        newMinimapRoom.GetComponent<MiniRoom>().SetActiveHall(directionFromNewRoom);
    }

    public void SpawnMiniStartRoom(Room startRoom)
    {
        MiniRoom miniStartRoom = Instantiate(miniRoomPrefab, miniRoomsHolder).GetComponent<MiniRoom>();
        miniStartRoom.transform.localPosition = Vector3.zero;
        miniStartRoom.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        _miniRooms = new Dictionary<Room, MiniRoom>() { { startRoom, miniStartRoom } };
    }

    private MiniRoom _lastSavedMiniRoom;
    public MiniRoom SetActiveMiniRoom (bool isActive, Room currentRoom)
    {
       MiniRoom currentMiniRoom = _miniRooms[currentRoom];
       currentMiniRoom.SetActiveMiniRoom(isActive);

       if (_lastSavedMiniRoom && _lastSavedMiniRoom != currentMiniRoom)
           _lastSavedMiniRoom.SetActiveMiniRoom(false);

       _lastSavedMiniRoom = currentMiniRoom;
       return currentMiniRoom;
    }
}

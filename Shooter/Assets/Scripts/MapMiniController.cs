using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MapMiniController : MonoBehaviour
{
    [SerializeField] private MiniRoom miniRoomPrefab;
    [SerializeField] private Transform miniRoomsHolder;
    [SerializeField] private Transform mapMiniRotateHolder;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Sprite[] awardTypes;

    public Transform MiniRoomsHolder { get { return miniRoomsHolder; } private set { miniRoomsHolder = value; } }

    private Dictionary<Room, MiniRoom> _miniRooms = new Dictionary<Room, MiniRoom>();
    private MiniRoom _lastSavedMiniRoom;

    private void Update()
    {
        mapMiniRotateHolder.localRotation = Quaternion.Euler(new Vector3(0, 0, playerTransform.localRotation.eulerAngles.y));
    }

    public void SpawnMiniMap(Vector3 spawnPosition, Vector2Int directionFromNewRoom, KeyValuePair<Vector2Int, Room> realRoom, KeyValuePair<Vector2Int, Room> parentRoom)
    {
        MiniRoom newMinimapRoom = Instantiate(miniRoomPrefab, miniRoomsHolder).GetComponent<MiniRoom>();
        newMinimapRoom.transform.localPosition = new Vector3(spawnPosition.x, spawnPosition.z, 0);
        newMinimapRoom.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        newMinimapRoom.SetAwardType(awardTypes[(int)realRoom.Value.GetRoomsAwardType()]);
        newMinimapRoom.gameObject.SetActive(false);
        _miniRooms.Add(realRoom.Value, newMinimapRoom);
        newMinimapRoom.GetComponent<MiniRoom>().SetActiveHall(directionFromNewRoom);
        UpdateMiniRoomMap(newMinimapRoom, realRoom, parentRoom, directionFromNewRoom);
    }

    public void SpawnMiniStartRoom(Room startRoom)
    {
        MiniRoom miniStartRoom = Instantiate(miniRoomPrefab, miniRoomsHolder).GetComponent<MiniRoom>();
        miniStartRoom.transform.localPosition = Vector3.zero;
        miniStartRoom.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        _miniRooms = new Dictionary<Room, MiniRoom>() { { startRoom, miniStartRoom } };
    }

    public MiniRoom SetActiveMiniRoom (bool isActive, Room currentRoom)
    {
       MiniRoom currentMiniRoom = _miniRooms[currentRoom];
       currentMiniRoom.SetActiveMiniRoom(isActive);

       if (_lastSavedMiniRoom && _lastSavedMiniRoom != currentMiniRoom)
           _lastSavedMiniRoom.SetActiveMiniRoom(false);

       _lastSavedMiniRoom = currentMiniRoom;
        UpdateMiniMapVision(currentMiniRoom);
       return currentMiniRoom;
    }

    public void UpdateMiniMapVision(MiniRoom currentRoom)
    {
        currentRoom.gameObject.SetActive(true);
        for (int i = 0; i < currentRoom.Neighours.Count; i++)
        {
            var naighbour = currentRoom.Neighours.ElementAt(i);
            naighbour.Value.gameObject.SetActive(true);
            naighbour.Value.SetTransparency();
        }
    }

    public void UpdateMiniRoomMap(MiniRoom spawnedRoom, KeyValuePair<Vector2Int, Room> realRoom, KeyValuePair<Vector2Int, Room> parentRoom, Vector2Int directionFromNewRoom)
    {
        if (_miniRooms.ContainsKey(realRoom.Value))
        {
            for (int i = 0; i < realRoom.Value.Neighours.Count; i++)
            {
                var naighbour = realRoom.Value.Neighours.Keys.ElementAt(i);
                if (!_miniRooms[realRoom.Value].Neighours.ContainsKey(naighbour))
                {
                    _miniRooms[realRoom.Value].Neighours.Add(naighbour, spawnedRoom);
                }
            }
        }

        if (_miniRooms.ContainsKey(parentRoom.Value))
        {
            if (!_miniRooms[parentRoom.Value].Neighours.ContainsKey(directionFromNewRoom))
            {
                _miniRooms[parentRoom.Value].Neighours.Add(directionFromNewRoom, spawnedRoom);
            }
            else if (!_miniRooms[parentRoom.Value].Neighours.ContainsKey(-directionFromNewRoom))
            {
                _miniRooms[parentRoom.Value].Neighours.Add(-directionFromNewRoom, spawnedRoom);
            }
        }
    }
}

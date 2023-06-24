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

    private Dictionary<Vector2Int, GameObject> _miniRooms;

    private void Update()
    {
        mapMiniRotateHolder.localRotation = Quaternion.Euler(new Vector3(0, 0, playerTransform.localRotation.eulerAngles.y));
    }

    public void SpawnMiniMap(Vector3 spawnPosition, Vector2Int directionFromNewRoom, Vector2Int parentRoom)
    {
        GameObject newMinimapRoom = Instantiate(miniRoomPrefab, miniRoomsHolder);
        newMinimapRoom.transform.localPosition = new Vector3(spawnPosition.x, spawnPosition.z, 0);
        newMinimapRoom.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        _miniRooms.Add(new Vector2Int(parentRoom.x, parentRoom.y), newMinimapRoom);
        newMinimapRoom.GetComponent<MiniRoom>().SetActiveHall(directionFromNewRoom);
    }

    public void SpawnMiniStartRoom()
    {
        GameObject miniStartRoom = Instantiate(miniRoomPrefab, miniRoomsHolder);
        miniStartRoom.transform.localPosition = Vector3.zero;
        miniStartRoom.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        _miniRooms = new Dictionary<Vector2Int, GameObject>() { { Vector2Int.zero, miniStartRoom } };
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMiniController : MonoBehaviour
{
    [SerializeField] private GameObject miniRoomPrefab;

    private Dictionary<Vector2Int, GameObject> _miniRooms;
    
    public void SpawnMiniMap(Vector3 spawnPosition, Vector2Int directionFromNewRoom, Vector2Int parentRoom)
    {
        GameObject newMinimapRoom = Instantiate(miniRoomPrefab, transform);
        newMinimapRoom.transform.localPosition = new Vector3(spawnPosition.x, spawnPosition.z, 0);
        newMinimapRoom.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        _miniRooms.Add(new Vector2Int(parentRoom.x, parentRoom.y), newMinimapRoom);
        newMinimapRoom.GetComponent<MiniRoom>().SetActiveHall(directionFromNewRoom);
    }

    public void SpawnMiniStartRoom()
    {
        GameObject miniStartRoom = Instantiate(miniRoomPrefab, transform);
        miniStartRoom.transform.localPosition = Vector3.zero;
        miniStartRoom.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        _miniRooms = new Dictionary<Vector2Int, GameObject>() { { Vector2Int.zero, miniStartRoom } };
    }
}

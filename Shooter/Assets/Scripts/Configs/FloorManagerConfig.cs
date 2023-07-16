using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FloorManager/Config")]

[System.Serializable]
public class FloorManagerConfig : ScriptableObject
{
    [SerializeField] public List<RoomSpawnerInfo> roomSpawnerConfigs = new List<RoomSpawnerInfo>();
}

[System.Serializable]
public struct RoomSpawnerInfo
{
    public int NumberOfRooms;
    public Vector2 RoomSize;
    public Room StartRoom;
    public Room EndRoom;
    public List<Room> BattleRoomPrefabs;
}

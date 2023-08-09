using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloormanagerConfig", menuName = "ScriptableObjects/FloorManagerConfig")]

[System.Serializable]
public class FloorManagerConfig : ScriptableObject
{
    public List<RoomSpawnerInfo> roomSpawnerConfigs = new List<RoomSpawnerInfo>();
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

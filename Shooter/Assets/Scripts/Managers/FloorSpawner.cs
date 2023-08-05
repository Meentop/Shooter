using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class FloorSpawner : MonoBehaviour
{
    [SerializeField] private RoomSpawner roomSpawner;
    [SerializeField] private FloorManagerConfig floorManagerConfig;
    [SerializeField] private Load load;

    private int _currentFloorNumber;

    public void UpdateFloor(int currentFloorNumber)
    {
        _currentFloorNumber = currentFloorNumber;
        roomSpawner.SetRoomSpawnerInfo(floorManagerConfig.roomSpawnerConfigs[currentFloorNumber]);
        roomSpawner.Init();
    }
    public int GetFloorCount() => _currentFloorNumber;
}
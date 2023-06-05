using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int place;

    [SerializeField] private GameObject[] doors;
    
    public void SetOpenDoor(Vector2Int direction)
    {
        doors[Array.IndexOf(RoomSpawner.directions, direction)].SetActive(false);
    }
}

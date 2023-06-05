using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    [HideInInspector] public Vector2Int place;

    [SerializeField] private GameObject[] doors;

    [SerializeField] private Vector2Int[] directions;

    public void SetOpenDoor(Vector2Int direction)
    {
        print(direction);
        doors[Array.IndexOf(RoomSpawner.directions, direction)].SetActive(false);
    }

    public Vector2Int[] GetDoorDirections()
    {
        return directions;
    }

    public bool HasDoorInDirection(Vector2Int direction)
    {
        //print(direction);
        foreach (var dir in directions)
        {
            if (dir.x == direction.x && dir.y == direction.y)
                return true;
        }
        return false;
    }
}

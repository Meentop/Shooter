using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int place;

    [SerializeField] private GameObject[] doors;
    Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };

    public void SetOpenDoor(Vector2Int direction)
    {
        doors[Array.IndexOf(directions, direction)].SetActive(false);
    }
}

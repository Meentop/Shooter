using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MiniRoom : MonoBehaviour
{
    private readonly Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    [SerializeField] private GameObject[] halls;
    public void SetActiveHall(Vector2Int direction)
    {
        int index = Array.IndexOf(directions, direction);
        halls[index].SetActive(true);
    }
}

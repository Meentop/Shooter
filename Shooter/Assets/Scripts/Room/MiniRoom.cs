using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MiniRoom : MonoBehaviour
{
    private readonly Vector2Int[] directions = { Vector2Int.right,Vector2Int.left, Vector2Int.down, Vector2Int.up };

    [SerializeField] private GameObject[] halls;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveHall(Vector2Int direction)
    {
        int index = Array.IndexOf(directions, direction);
        halls[index].SetActive(true);
    }
}

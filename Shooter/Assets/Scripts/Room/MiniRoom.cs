using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MiniRoom : MonoBehaviour
{
    private readonly Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    [SerializeField] private GameObject[] halls;
    [SerializeField] private Color unactiveColor;
    [SerializeField] private Color activeColor;

    public Dictionary<Vector2Int, MiniRoom> Neighours = new Dictionary<Vector2Int, MiniRoom>();

    private bool _isActive;
    public void SetActiveHall(Vector2Int direction)
    {
        int index = Array.IndexOf(directions, direction);
        halls[index].SetActive(true);
    }

    public void SetColor(bool isActive)
    {
        if (isActive)
            gameObject.GetComponent<Image>().color = activeColor;
        else
            gameObject.GetComponent<Image>().color = unactiveColor;
    }

    public void SetActiveMiniRoom(bool isActive)
    {
        _isActive = isActive;
        SetColor(_isActive);
    }
}

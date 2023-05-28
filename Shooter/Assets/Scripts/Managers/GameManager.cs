using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private InfoInterface InfoInterface;
    private void Awake()
    {
        player.Init(InfoInterface);
    }
}

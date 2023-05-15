using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uIManager;
    [SerializeField] private Player player;
    private void Awake()
    {
        player.Init(uIManager);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Player player;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private RoomSpawner roomSpawner;
    private void Awake()
    {
        player.Init(uiManager, cameraController);
        roomSpawner.Init();
    }
}

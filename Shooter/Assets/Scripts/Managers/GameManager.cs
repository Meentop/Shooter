using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Player player;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private RoomSpawner roomSpawner;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private Transform modifierDragHolder;
    [SerializeField] private ModifiersManager modifiersManager;

    private void Awake()
    {
        player.Init(uiManager, cameraController, mainCamera, canvas, modifierDragHolder, modifiersManager);
        roomSpawner.Init();
    }
}

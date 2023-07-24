using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Player player;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private RoomSpawner roomSpawner;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private FloorSpawner floorSpawner;

    private void Awake()
    {
        player.Init(uiManager, cameraController, mainCamera, canvas);
        floorSpawner.UpdateFloor(player);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            floorSpawner.SaveToFile("saveData.json");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

public class SaveData
{
    public int currentFlorNumber;
}

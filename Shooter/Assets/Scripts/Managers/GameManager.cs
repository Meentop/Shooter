using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    [SerializeField] private Load load;
    [SerializeField] private Save save;

    private void Awake()
    {
        load.LoadFromFile("saveData.json");
        player.Init(uiManager, cameraController, mainCamera, canvas);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            save.SaveToFile("saveData.json");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.R))
        { 
            Debug.Log("RABOTAET");
            DeleteFromFile("saveData.json");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void DeleteFromFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}

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
    [SerializeField] private EnemiesTeamVariationsConfig enemiesTeamVariationsConfig;


    private const string saveName = "saveData.json";
    private bool death;

    private void Awake()
    {
        ObjectPool.Instance.ClearPool();
        player.Init(this, uiManager, cameraController, mainCamera, canvas);
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Hub"))
            DeleteFromFile(saveName);
        load.LoadFromFile(saveName);
        enemiesTeamVariationsConfig.curEnemiesPool = enemiesTeamVariationsConfig.enemyPoolVariations[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            LoadNextFloor();
        if (Input.GetKeyDown(KeyCode.R))
        { 
            DeleteFromFile(saveName);
            SceneManager.LoadScene("Hub");
        }
        if(death && Input.anyKeyDown)
            SceneManager.LoadScene("Hub");
    }

    public void DeleteFromFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public void LoadNextFloor()
    {
        save.SaveToFile(saveName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetActiveDeathReload()
    {
        StartCoroutine(DeathReload());
    }

    IEnumerator DeathReload()
    {
        yield return new WaitForSeconds(4);
        death = true;
    }

    public void DeleteSave()
    {
        DeleteFromFile(saveName);
    }
}

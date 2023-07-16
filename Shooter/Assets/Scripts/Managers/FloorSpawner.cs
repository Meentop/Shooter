using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class FloorSpawner : MonoBehaviour
{
    [SerializeField] private RoomSpawner roomSpawner;
    [SerializeField] private FloorManagerConfig floorManagerConfig;

    private SaveData _saveData = new SaveData();

    public void UpdateFloor(Player player)
    {
        try {
            LoadFromFile("saveData.json");
            roomSpawner.SetRoomSpawnerInfo(floorManagerConfig.roomSpawnerConfigs[_saveData.currentFlorNumber]);
            _saveData.currentFlorNumber++;
            roomSpawner.Init();
            Debug.Log(_saveData.currentFlorNumber + "1111111");
        }
        catch
        {
            Debug.Log(_saveData.currentFlorNumber);
        }
    }

    public void SaveToFile(string fileName)
    {
        string json = JsonUtility.ToJson(_saveData);
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.Write(json);
        }
    }

    public void LoadFromFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string json = reader.ReadToEnd();
                SaveData saveData = new SaveData();
                JsonUtility.FromJsonOverwrite(json, saveData);
                _saveData = saveData;
            }
        }
    }
}
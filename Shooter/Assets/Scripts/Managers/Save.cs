using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Save : MonoBehaviour
{
    [SerializeField] private FloorSpawner floorSpawner;
    [SerializeField] private Player player;

    private SaveData _saveData = new SaveData();

    public void SaveToFile(string fileName)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Run"))
        {
            _saveData.currentFlorNumber = floorSpawner.GetFloorCount() + 1;
            _saveData.currentPlayerHP = player.Health.GetHealth();
        }
        else if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Hub"))
        {
            _saveData.currentFlorNumber = 0;
            _saveData.currentPlayerHP = player.Health.GetMaxHealth();
        }

        _saveData.weapons = player.GetWeaponSaves();

        string json = JsonUtility.ToJson(_saveData);
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        using StreamWriter writer = new StreamWriter(filePath);
        writer.WriteLine(json);
    }
}

[System.Serializable]
public struct SaveData
{
    public int currentFlorNumber;
    public int currentPlayerHP;
    public List<WeaponSave> weapons;
}
[System.Serializable]
public struct WeaponSave
{
    public int number;
    public int level;
    public List<WeaponModuleSave> modules;
}
[System.Serializable]
public struct WeaponModuleSave
{
    public int number;
    public int level;
}
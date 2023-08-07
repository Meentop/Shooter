using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{
    [SerializeField] private FloorSpawner floorSpawner;
    [SerializeField] private PlayerHealth playerHealth;

    private SaveData _saveData = new SaveData();

    public void SaveToFile(string fileName)
    {
        _saveData.currentFlorNumber = floorSpawner.GetFloorCount() + 1;
        _saveData.currentPlayerHP = playerHealth.GetPlayerHelth();
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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Load : MonoBehaviour
{
    [SerializeField] private FloorSpawner floorSpawner;
    [SerializeField] private PlayerHealth playerHealth;

    public void LoadFromFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string json = reader.ReadToEnd();
                SaveData saveData = new SaveData();
                saveData = JsonUtility.FromJson<SaveData>(json);
                floorSpawner.UpdateFloor(saveData.currentFlorNumber);
            }
        }
        else
        {
            floorSpawner.UpdateFloor(new Vector3Int(Vector3Int.zero.x, Vector3Int.zero.y, Vector3Int.zero.z).y);
        }
    }
}

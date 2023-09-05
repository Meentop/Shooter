using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Load : MonoBehaviour
{
    [SerializeField] private FloorSpawner floorSpawner;
    [SerializeField] private Player player;

    public void LoadFromFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            using StreamReader reader = new StreamReader(filePath);
            string json = reader.ReadToEnd();
            SaveData saveData = new SaveData();
            saveData = JsonUtility.FromJson<SaveData>(json);
            floorSpawner.UpdateFloor(saveData.currentFlorNumber);
            player.Health.SetCurHealth(saveData.currentPlayerHP);
            player.LoadWeapons(saveData.weapons);
            player.LoadFreeWeaponModules(saveData.freeWeaponModules);
            player.Gold.Add(saveData.gold);
            player.LoadInstalledBionicModules(saveData.installedBionicModules);
            player.LoadFreeBionicModules(saveData.freeBionicModules);
        }
        else
        {
            player.Health.SetCurHealthToMax();
            List<WeaponSave> weaponSaves = new List<WeaponSave> 
            { 
                new WeaponSave { number = 0, level = 0, modules = new List<WeaponModuleSave>() }, 
                new WeaponSave { number = 0, level = 0, modules = new List<WeaponModuleSave>() } 
            };
            player.LoadWeapons(weaponSaves);
        }
    }
}

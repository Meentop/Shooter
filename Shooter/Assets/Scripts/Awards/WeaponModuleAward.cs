using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModuleAward : MonoBehaviour
{
    [SerializeField] private Transform[] stands;
    [SerializeField] private WeaponModuleConfig config;
    [SerializeField] private float moduleHeight;

    private void Start()
    {
        List<WeaponModule> modules = new List<WeaponModule>();
        modules.AddRange(config.Modules);
        foreach (var stand in stands)
        {
            int randomNumber = Random.Range(0, modules.Count);
            WeaponModule modifier = modules[randomNumber];
            modules.RemoveAt(randomNumber);
            Transform moduleTransform = Instantiate(modifier, stand).transform;
            moduleTransform.localPosition = new Vector3(0, moduleHeight, 0);
        }
    }

    public void DeleteOtherModules(Transform thisStand)
    {
        foreach (var stand in stands)
        {
            if (stand != thisStand && stand.childCount > 0)
                Destroy(stand.GetComponentInChildren<WeaponModule>().gameObject);
        }
    }
}

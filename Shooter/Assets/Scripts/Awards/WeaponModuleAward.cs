using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModuleAward : MonoBehaviour
{
    [SerializeField] private Transform[] stands;
    [SerializeField] private List<WeaponModule> modules;
    [SerializeField] private float moduleHeight;

    private void Start()
    {
        foreach (var stand in stands)
        {
            int randomNumber = Random.Range(0, modules.Count);
            WeaponModule modifier = modules[randomNumber];
            modules.RemoveAt(randomNumber);
            Transform modifierTransform = Instantiate(modifier, stand).transform;
            modifierTransform.localPosition = new Vector3(0, moduleHeight, 0);
        }
    }

    public void DeleteOtherModifiers(Transform thisStand)
    {
        foreach (var stand in stands)
        {
            if (stand != thisStand && stand.childCount > 0)
                Destroy(stand.GetComponentInChildren<WeaponModule>().gameObject);
        }
    }
}

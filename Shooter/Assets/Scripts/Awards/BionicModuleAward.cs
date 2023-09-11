using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BionicModuleAward : MonoBehaviour
{
    [SerializeField] private Transform[] stands;
    [SerializeField] private BionicModuleConfig config;
    [SerializeField] private float moduleHeight;

    private void Start()
    {
        List<BionicModule> modules = new List<BionicModule>();
        modules.AddRange(config.Modules);
        foreach (var stand in stands)
        {
            int randomNumber = Random.Range(0, modules.Count);
            BionicModule modifier = modules[randomNumber];
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
                Destroy(stand.GetComponentInChildren<BionicModule>().gameObject);
        }
    }
}

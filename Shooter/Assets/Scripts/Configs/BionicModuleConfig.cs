using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BionicModuleConfig", menuName = "ScriptableObjects/BionicModuleConfig")]
public class BionicModuleConfig : ScriptableObject
{
    [SerializeField] private List<BionicModule> modules = new List<BionicModule>();
    [HideInInspector] public List<BionicModule> Modules { get => modules; }

    public int GetIndex(BionicModule module)
    {
        foreach (BionicModule module2 in modules)
        {
            if (module2.GetTitle(0) == module.GetTitle(0))
                return modules.IndexOf(module2);
        }
        return -1;
    }
}

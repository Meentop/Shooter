using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BionicModuleConfig", menuName = "ScriptableObjects/BionicModuleConfig")]
public class BionicModuleConfig : ScriptableObject
{
    [SerializeField] private List<BionicModule> modules = new List<BionicModule>();
    [HideInInspector] public List<BionicModule> Modules { get => modules; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillConfig", menuName = "ScriptableObjects/ActiveSkillConfig")]
public class ActiveSkillConfig : ScriptableObject
{
    [SerializeField] private List<ActiveSkill> activeSkills = new List<ActiveSkill>();
    [HideInInspector] public List<ActiveSkill> ActiveSkills { get => activeSkills; }
}

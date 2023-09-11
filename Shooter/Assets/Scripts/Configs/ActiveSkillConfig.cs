using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillConfig", menuName = "ScriptableObjects/ActiveSkillConfig")]
public class ActiveSkillConfig : ScriptableObject
{
    [SerializeField] private List<ActiveSkill> activeSkills = new List<ActiveSkill>();
    [HideInInspector] public List<ActiveSkill> ActiveSkills { get => activeSkills; }

    public int GetIndex(ActiveSkill skill)
    {
        foreach (ActiveSkill skill2 in activeSkills)
        {
            if(skill2.GetTitle() == skill.GetTitle())
                return activeSkills.IndexOf(skill2);
        }
        return -1;
    }
}

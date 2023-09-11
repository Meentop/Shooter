using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillAward : MonoBehaviour
{
    [SerializeField] private Transform[] stands;
    [SerializeField] private ActiveSkillConfig config;
    [SerializeField] private float activeSkillHeight;

    private void Start()
    {
        List<ActiveSkill> skills = new List<ActiveSkill>();
        skills.AddRange(config.ActiveSkills);
        foreach (var stand in stands)
        {
            int randomNumber = Random.Range(0, skills.Count);
            ActiveSkill activeSkill = skills[randomNumber];
            skills.RemoveAt(randomNumber);
            Transform modifierTransform = Instantiate(activeSkill, stand).transform;
            modifierTransform.localPosition = new Vector3(0, activeSkillHeight, 0);
        }
    }

    public void DeleteOtherSkills(Transform thisStand)
    {
        foreach (var stand in stands)
        {
            if (stand != thisStand && stand.childCount > 0)
                Destroy(stand.GetComponentInChildren<ActiveSkill>().gameObject);
        }
    }
}

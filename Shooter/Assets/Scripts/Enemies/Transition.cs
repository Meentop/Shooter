using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public List<Condition> conditions;
    public AgentState target;

    public bool TestAllConditions()
    {
        foreach (var condition in conditions)
        {
            if(!condition.Test())
                return false;
        }
        return true;
    }
}

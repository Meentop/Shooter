using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ModifierBehaviour : MonoBehaviour
{
    public abstract DamageData ApplyBehaviour(DamageData damageData);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BionicModuleBehaviour : MonoBehaviour
{
    public abstract PlayerMovement ApplyBehaviour(PlayerMovement movement, InfoForBionicModule info);
}

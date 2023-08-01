using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition : MonoBehaviour
{
    protected Player player;

    public abstract bool Test();

    public void Init(Player player)
    {
        this.player = player;
    }
}

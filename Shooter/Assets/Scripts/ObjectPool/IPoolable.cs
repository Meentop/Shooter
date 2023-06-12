using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    GameObject GameObject { get; }
    event Action<IPoolable> Destroyed;
    void Reset();
}

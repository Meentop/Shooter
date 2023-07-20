using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : ActiveSkill
{
    [SerializeField] private GameObject fireball;

    protected override void OnActivated()
    {
        Instantiate(fireball, mainCamera.position, mainCamera.rotation);
    }
}

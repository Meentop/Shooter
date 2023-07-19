using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBall : ActiveSkill
{
    [SerializeField] private GameObject teleportBall;

    protected override void OnActivated()
    {
        Instantiate(teleportBall, mainCamera.position, mainCamera.rotation);
    }
}

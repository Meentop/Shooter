using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBall : ActiveSkill
{
    [SerializeField] private TeleportProjectile teleportBall;

    protected override void OnActivated()
    {
        TeleportProjectile projectile = Instantiate(teleportBall, mainCamera.position, mainCamera.rotation);
        projectile.Init(player);
    }
}

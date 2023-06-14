using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFromRoomAction : ActionBase
{
    [SerializeField] private Enemy enemy;

    [HideInInspector] public Room room;

    public override void ExecuteAction(params ActionParameter[] parameters)
    {
        if(room != null)
            room.RemoveEnemy(enemy);
    }
}

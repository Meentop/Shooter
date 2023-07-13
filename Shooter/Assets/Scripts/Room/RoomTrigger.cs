using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private Room room;

    private bool enemiesActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            room.UpdateMiniMapPos(1f);
            room.SetActiveMiniRoom(true);
        }
        if (room.IsBattleRoom() && !enemiesActivated && other.gameObject.GetComponent<Player>())
        {
            room.SpawnEnemies();
            room.SetDoors(true);
            enemiesActivated = true;
        }       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            
        }
    }
}

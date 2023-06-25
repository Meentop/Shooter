using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private Room room;
    [SerializeField] private bool battleRoom;

    private bool enemiesActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            room.UpdateMiniMapPos(1f);
        }
        if (battleRoom && !enemiesActivated && other.gameObject.GetComponent<Player>())
        {
            room.SpawnEnemies();
            room.SetDoors(true);
            enemiesActivated = true;
        }       
    }
}

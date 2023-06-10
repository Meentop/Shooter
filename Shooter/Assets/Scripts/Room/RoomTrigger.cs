using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private Room room;

    private bool enemiesActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!enemiesActivated && room.GetEnemyCount() > 0 && other.gameObject.GetComponent<Player>())
        {
            room.ActivateEnemies();
            room.SetDoors(true);
            enemiesActivated = true;
        }
    }
}

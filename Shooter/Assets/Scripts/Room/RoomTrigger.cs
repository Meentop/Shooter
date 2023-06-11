using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private Room room;

    private bool enemiesActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!enemiesActivated && other.gameObject.GetComponent<Player>())
        {
            room.SpawnEnemies();
            room.SetDoors(true);
            enemiesActivated = true;
        }
    }
}

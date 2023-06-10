using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPassage : MonoBehaviour
{
    private Blackout blackout;

    private void Start()
    {
        blackout = Blackout.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            blackout.OnBlackout();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            blackout.OffBlackout();
        }
    }
}

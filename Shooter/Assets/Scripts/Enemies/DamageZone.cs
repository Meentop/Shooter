using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Player>())
        {
            if (other.gameObject.transform.TryGetComponent<IDamageReceiver>(out var damageReceiver))
            {
                DamageData damageData = new DamageData(FindAnyObjectByType<Player>(), damage: damage);
                damageReceiver.GetDamage(damageData);
            }
        }
    }
}

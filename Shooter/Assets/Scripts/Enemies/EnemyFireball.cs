using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireball : MonoBehaviour, IPoolable
{
    [SerializeField] private float speed;
    [SerializeField] private float maxRadiansDelta;
    public GameObject GameObject => gameObject;

    public event Action<IPoolable> Destroyed;

    Transform player;

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
        transform.rotation = Quaternion.LookRotation((player.position - transform.position).normalized);
    }

    private void Update()
    {
        if (!PauseManager.Pause)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.forward);

            Vector3 targetDirection = player.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, maxRadiansDelta, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            DamageData damageData = new DamageData(damage: 10);
            other.gameObject.GetComponent<BaseDamageReceiver>().GetDamage(damageData);
            Reset();
        }
        else if(other.gameObject.CompareTag("Solid"))
        {
            Reset();
        }
    }

    public void Reset()
    {
        Destroyed?.Invoke(this);
    }
}

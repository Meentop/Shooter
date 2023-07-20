using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody _rb;
    private Player _player;

    public void Init(Player player)
    {
        _rb = GetComponent<Rigidbody>();
        _player = player;
    }

    private void FixedUpdate()
    {
        _rb.velocity = speed * transform.TransformDirection(Vector3.forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _player.transform.position = transform.position;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        _player.transform.position = transform.position;
        Destroy(gameObject);
    }
}

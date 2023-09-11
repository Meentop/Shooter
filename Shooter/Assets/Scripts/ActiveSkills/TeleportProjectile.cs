using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportProjectile : MonoBehaviour, IPauseble
{
    [SerializeField] private float speed;
    private Rigidbody _rb;
    private Player _player;

    private Vector3 prepauseVelocity;

    public void Init(Player player)
    {
        _rb = GetComponent<Rigidbody>();
        _player = player;
        PauseManager.OnSetPause += OnSetPause;
    }

    private void FixedUpdate()
    {
        if (!PauseManager.Pause)
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

    public void OnSetPause(bool value)
    {
        if (value)
        {
            prepauseVelocity = _rb.velocity;
            _rb.velocity = Vector3.zero;
        }
        else
        {
            _rb.velocity = prepauseVelocity;
        }
    }

    private void OnDestroy()
    {
        PauseManager.OnSetPause -= OnSetPause;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpSphere : MonoBehaviour
{
    [SerializeField] private float pickDistance, speed;
    public int exp;

    private Transform _player;
    private Rigidbody _rb;
    private Collider _collider;

    private bool _isPicked;

    private void Start()
    {
        _player = FindObjectOfType<Player>().transform;
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!_isPicked && Vector3.Distance(_player.position, transform.position) <= pickDistance)
        {
            _rb.useGravity = false;
            _collider.isTrigger = true;
            _isPicked = true;
        }    
    }

    private void FixedUpdate()
    {
        if (_isPicked)
        {
            _rb.AddForce((_player.position - transform.position).normalized * speed, ForceMode.Force);
        }
    }
}

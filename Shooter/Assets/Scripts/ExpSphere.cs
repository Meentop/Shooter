using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpSphere : MonoBehaviour
{
    [SerializeField] private float pickDistance, speed, lifeTime;
    public int exp;

    private Transform _player;
    private Rigidbody _rb;
    private Collider _collider;
    private float _timer;

    private bool _isDroped, _isPicked;

    public event Action<ExpSphere> Destroyed;

    private void Start()
    {
        _player = FindObjectOfType<Player>().transform;
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if(_isDroped && !_isPicked)
        {
            _timer += Time.deltaTime;
            if (_timer >= lifeTime)
                Destroy();
        }
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
            //_rb.AddForce((_player.position - transform.position).normalized * speed, ForceMode.Force);
            _rb.velocity = (_player.position - transform.position).normalized * speed;
        }
    }

    public void StartTimer()
    {
        _isDroped = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerLvl>(out var lvl))
        {
            Destroy(lvl);
        }
    }

    private void Destroy(PlayerLvl lvl)
    {
        ResetAll();

        lvl.AddExp(exp);
        Destroyed?.Invoke(this);
    }

    private void Destroy()
    {
        ResetAll();

        Destroyed?.Invoke(this);
    }

    private void ResetAll()
    {
        _rb.useGravity = true;
        _collider.isTrigger = false;
        _isPicked = false;
        _rb.velocity = Vector3.zero;
        _isDroped = false;
        _timer = 0;
    }
}

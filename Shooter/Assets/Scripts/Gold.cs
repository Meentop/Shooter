using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour, IPoolable
{
    [SerializeField] private float pickDistance, speed, lifeTime, unpullableTime;
    public GameObject GameObject => gameObject;

    private Transform _player;
    private Rigidbody _rb;
    private Collider _collider;

    private bool _isPicked, _canPull;
    private float _lifeTimer, _unpullableTimer;

    public event Action<IPoolable> Destroyed;

    private void Start()
    {
        _player = FindObjectOfType<Player>().transform;
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!_isPicked)
        {
            _lifeTimer += Time.deltaTime;
            if (_lifeTimer >= lifeTime)
                Reset();
        }
        if(!_isPicked && !_canPull)
        {
            _unpullableTimer += Time.deltaTime;
            if(_unpullableTimer >= unpullableTime)
                _canPull = true;
        }
        if (!_isPicked && _canPull && Vector3.Distance(_player.position, transform.position) <= pickDistance)
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
            _rb.velocity = (_player.position - transform.position).normalized * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            PlayerGold.Instance.AddGold();
            Reset();
        }
    }

    public void Reset()
    {
        ResetAllCharacteristics();
        Destroyed?.Invoke(this);
    }

    private void ResetAllCharacteristics()
    {
        _rb.useGravity = true;
        _collider.isTrigger = false;
        _isPicked = false;
        _canPull = false;
        _rb.velocity = Vector3.zero;
        _lifeTimer = 0;
        _unpullableTimer = 0;
    }
}

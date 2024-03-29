using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour, IPoolable, IPauseble
{
    [SerializeField] private int goldCount;
    [SerializeField] private float pullDistance, pickDistance, speed, lifeTime, unpullableTime;
    public GameObject GameObject => gameObject;

    private Transform _player;
    private Rigidbody _rb;
    private Collider _collider;

    private bool _isPicked, _canPull;
    private float _lifeTimer, _unpullableTimer;

    public event Action<IPoolable> Destroyed;
    private Vector3 prepauseVelocity;

    private void Start()
    {
        _player = FindObjectOfType<Player>().transform;
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        PauseManager.OnSetPause += OnSetPause;
    }

    private void Update()
    {
        if (!_isPicked && !PauseManager.Pause)
        {
            _lifeTimer += Time.deltaTime;
            if (_lifeTimer >= lifeTime)
                Reset();
        }
        if(!_isPicked && !_canPull && !PauseManager.Pause)
        {
            _unpullableTimer += Time.deltaTime;
            if(_unpullableTimer >= unpullableTime)
                _canPull = true;
        }
        if (!_isPicked && _canPull && Vector3.Distance(_player.position, transform.position) <= pullDistance)
        {
            _rb.useGravity = false;
            _collider.isTrigger = true;
            _isPicked = true;
        }
        if (_isPicked && Vector3.Distance(_player.position, transform.position) <= pickDistance)
        {
            _player.GetComponent<Player>().Gold.Add(goldCount);
            Reset();
        }
    }

    private void FixedUpdate()
    {
        if (_isPicked && !PauseManager.Pause)
        {
            _rb.velocity = (_player.position - transform.position).normalized * speed;
        }
    }

    private void OnDestroy()
    {
        PauseManager.OnSetPause -= OnSetPause;
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

    public void OnSetPause(bool value)
    {
        if (value)
        {
            prepauseVelocity = _rb.velocity;
            _rb.isKinematic = true;
        }
        else
        {
            _rb.isKinematic = false;
            _rb.velocity = prepauseVelocity;
        }
    }
}

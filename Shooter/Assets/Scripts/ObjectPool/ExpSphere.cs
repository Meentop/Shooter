using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpSphere : MonoBehaviour, IPoolable
{
    [SerializeField] private float pickDistance, speed, lifeTime;
    [HideInInspector] public int exp;
    public GameObject GameObject => gameObject;

    private Transform _player;
    private Rigidbody _rb;
    private Collider _collider;
    private float _timer;

    private bool _isDroped, _isPicked;

    public event Action<IPoolable> Destroyed;

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
                Reset();
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
            _rb.velocity = (_player.position - transform.position).normalized * speed;
        }
    }

    private void OnEnable()
    {
        _isDroped = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerLvl>(out var lvl))
        {
            lvl.AddExp(exp);
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
        _rb.velocity = Vector3.zero;
        _isDroped = false;
        _timer = 0;
    }
}

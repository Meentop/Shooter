using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireball : MonoBehaviour, IPauseble
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private GameObject explosion;

    private Vector3 prepauseVelocity;

    private void Start()
    {
        PauseManager.OnSetPause += OnSetPause;
    }

    private void FixedUpdate()
    {
        if(!PauseManager.Pause)
            rb.velocity = speed * transform.TransformDirection(Vector3.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void OnSetPause(bool value)
    {
        if (value)
        {
            prepauseVelocity = rb.velocity;
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.velocity = prepauseVelocity;
        }
    }

    private void OnDestroy()
    {
        PauseManager.OnSetPause -= OnSetPause;
    }
}

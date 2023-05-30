using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireboll : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxRadiansDelta;

    Transform player;

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
        transform.rotation = Quaternion.LookRotation((player.position - transform.position).normalized);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Vector3 targetDirection = player.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, maxRadiansDelta, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<BaseDamageReceiver>().OnGetDamage(new DamageData(10f, new RaycastHit()));
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Solid")
        {
            Destroy(gameObject);
        }
    }
}

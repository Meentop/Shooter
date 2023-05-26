using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Rigidbody rb;
    protected Transform player;
    protected bool isAttacking; 

    [SerializeField] protected Animator anim;

    [SerializeField] protected float maxRadiansDelta;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<Player>().transform;
    }

    protected virtual void Update()
    {
        
    }

    protected float GetAngleToPlayer()
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 toPlayerXZ = new Vector3(toPlayer.x, 0, toPlayer.z);
        Vector3 forwardXZ = new Vector3(transform.forward.x, 0, transform.forward.z);
        return Vector3.Angle(toPlayerXZ, forwardXZ);
    }

    public void RotateToPlayer()
    {
        Vector3 playerPosXZ = new Vector3(player.position.x, 0, player.position.z);
        Vector3 transformPosXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetDirection = playerPosXZ - transformPosXZ;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, maxRadiansDelta, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}

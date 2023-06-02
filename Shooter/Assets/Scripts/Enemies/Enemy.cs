using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public abstract class Enemy : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Rigidbody rb;
    protected Transform player;
    protected bool isAttacking, canMove = true;
    protected NavMeshPath path;
    protected Vector3 moveDirection;

    [SerializeField] protected Animator anim;

    [SerializeField] protected float maxRadiansDelta, moveSpeed;

    //maximum distance to move away from other enemies
    [SerializeField] protected float trackingDistance;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<Player>().transform;
        path = new NavMeshPath();
    }

    protected virtual void Update()
    {
        if (canMove)
        {
            SetDirectionAlongPath();
            moveDirection = (GetCorrectionVector() + moveDirection).normalized;
            Rotate();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    protected void SetDirectionAlongPath()
    {
        NavMesh.CalculatePath(transform.position, player.position, NavMesh.AllAreas, path);
        if (path.corners.Length > 1)
        {
            moveDirection = (path.corners[1] - path.corners[0]).normalized;
        }
    }

    protected Vector3 GetCorrectionVector()
    {
        //get nearest enemies
        List<Collider> colliders = Physics.OverlapSphere(transform.position, trackingDistance, LayerMask.GetMask("Enemy")).ToList();
        List<Enemy> enemies = colliders.Select(enemy => enemy.GetComponentInParent<Enemy>()).ToList();
        enemies = enemies.Distinct().ToList();
        enemies.Remove(this);
        //get correction vector
        List<Vector3> correctionVectors = new List<Vector3>();
        foreach (var enemy in enemies)
        {
            float distancePower = (trackingDistance - Vector3.Distance(transform.position, enemy.transform.position)) / trackingDistance;
            Vector3 directionFromEnemy = (transform.position - enemy.transform.position).normalized;
            float dotProduct = Vector3.Dot(transform.right, directionFromEnemy.normalized);
            float correctionPower = distancePower * Mathf.Abs(dotProduct);
            correctionVectors.Add(directionFromEnemy * correctionPower);
            //Debug.DrawLine(transform.position, transform.position + (directionFromEnemy * correctionPower), Color.red);
        }
        Vector3 correctionVector = Vector3.zero;
        for (int i = 0; i < correctionVectors.Count; i++)
        {
            correctionVector += correctionVectors[i];
        }
        //Debug.DrawLine(transform.position, transform.position + correctionVector, Color.yellow);
        correctionVectors.Clear();
        return new Vector3(correctionVector.x, 0, correctionVector.z);
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

    protected void Move()
    {
        //check if moveDirection is on nav mesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position + moveDirection, out hit, 1.0f, NavMesh.AllAreas))
        {
            moveDirection = (hit.position - transform.position).normalized;
        }
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
        Debug.DrawLine(transform.position, transform.position + moveDirection, Color.yellow);
    }

    protected void Rotate()
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z).normalized);
    }
}

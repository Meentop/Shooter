using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Pursue : AgentState
{
    [SerializeField] protected float trackingDistance;

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, player.transform.position, NavMesh.AllAreas, path);
        if (path.corners.Length > 1)
        {
            steering.linear = (path.corners[1] - path.corners[0]).normalized;
        }
        steering.linear = (GetCorrectionVector() + steering.linear).normalized;

        Vector3 direction = agent.GetVelocity().normalized;
        if(direction.magnitude > 0)
        {
            float targetOrientation = Mathf.Atan2(direction.x, direction.z);
            targetOrientation *= Mathf.Rad2Deg;
            steering.orientation = targetOrientation;
        }

        return steering;
    }

    protected Vector3 GetCorrectionVector()
    {
        //get nearest enemies
        List<Collider> colliders = Physics.OverlapSphere(transform.position, trackingDistance, LayerMask.GetMask("Enemy")).ToList();
        List<Agent> enemies = colliders.Select(enemy => enemy.GetComponentInParent<Agent>()).ToList();
        enemies = enemies.Distinct().ToList();
        enemies.Remove(agent);
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

    protected override void OnDisable()
    {
        
    }

    protected override void OnEnable()
    {
        anim.SetBool("Move", true);
    }
}

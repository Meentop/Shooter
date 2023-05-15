using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField] private float movementSpeed;
    private bool isMove = true;

    [SerializeField] private float jumpStrength;

    [SerializeField] private Transform checkOnGroundPoint;

    [SerializeField] private float checkOnGroundRadius;

    [SerializeField] private float dashStrength, dashDuration, dashReloadTime;
    private float dashTimer = 0;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryJump();
        }

        if(dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0)
        {
            Dash();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(checkOnGroundPoint.position, checkOnGroundRadius);
    }

    private void FixedUpdate()
    {
        if(isMove)
            Movement();
    }

    private void Movement()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 movementDirection = transform.TransformDirection(movement).normalized * movementSpeed;

        _rb.velocity = new Vector3(movementDirection.x, _rb.velocity.y, movementDirection.z);
    }

    private void TryJump()
    {
        Vector3 overlapCenter = checkOnGroundPoint.position;

        Collider[] colliders = Physics.OverlapSphere(overlapCenter, checkOnGroundRadius, LayerMask.GetMask("Solid"));

        if (colliders.Length > 0)
            Jump();
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
    }

    private void Dash()
    {
        StartCoroutine(DashCor());
    }

    private IEnumerator DashCor()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            direction = Vector3.forward;
        isMove = false;
        dashTimer = dashReloadTime;
        float dashDuration = this.dashDuration;
        while(dashDuration > 0)
        {
            Vector3 dashDirection = transform.TransformDirection(direction) * dashStrength;
            _rb.velocity = new Vector3(dashDirection.x, _rb.velocity.y, dashDirection.z);
            dashDuration -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        isMove = true;
    }
}

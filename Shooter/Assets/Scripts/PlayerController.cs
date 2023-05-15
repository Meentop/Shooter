using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform checkOnGroundPoint;
    [SerializeField] private PlayerConfig playerConfig;
    [SerializeField] private Crossheir crossheir;

    private Rigidbody _rb;
    private bool _isMove = true;
    private float _dashTimer;

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

        if(_dashTimer > 0)
        {
            _dashTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _dashTimer <= 0)
        {
            Dash();
        }

        if(_rb.velocity == Vector3.zero)
        {
            crossheir.CurrentSpread = 20;
        }

        else
        {
            crossheir.CurrentSpread = 50;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(checkOnGroundPoint.position, playerConfig.checkOnGroundRadius);
    }

    private void FixedUpdate()
    {
        if(_isMove)
        {
            Movement();
        }
       
            
    }

    private void Movement()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 movementDirection = transform.TransformDirection(movement).normalized * playerConfig.movementSpeed;

        _rb.velocity = new Vector3(movementDirection.x, _rb.velocity.y, movementDirection.z);
    }

    private void TryJump()
    {
        Vector3 overlapCenter = checkOnGroundPoint.position;

        Collider[] colliders = Physics.OverlapSphere(overlapCenter, playerConfig.checkOnGroundRadius, LayerMask.GetMask("Solid"));

        if (colliders.Length > 0)
            Jump();
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * playerConfig.jumpStrength, ForceMode.Impulse);
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
        _isMove = false;
        this._dashTimer = playerConfig.dashReloadTime;
        float dashTimer = this.playerConfig.dashDuration;
        while(dashTimer > 0)
        {
            Vector3 dashDirection = transform.TransformDirection(direction) * playerConfig.dashStrength;
            _rb.velocity = new Vector3(dashDirection.x, _rb.velocity.y, dashDirection.z);
            dashTimer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        _isMove = true;
    }
}

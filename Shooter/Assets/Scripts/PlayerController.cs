using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform checkOnGroundPoint;
    [SerializeField] private PlayerConfig playerConfig;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    private Rigidbody _rb;
    private bool _canMove = true, _grounded;
    private float _dashTimer;
    private float _horizontalInput, _verticalInput;
    private Vector3 _moveDirection;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    private void Update()
    {
        _grounded = Physics.Raycast(transform.position, Vector3.down, 1.2f, LayerMask.GetMask("Solid"));

        MyInput();
        SpeedControl();

        if (_grounded)
            _rb.drag = playerConfig.groundDrag;
        else
            _rb.drag = 0;

        

        if(_dashTimer > 0)
        {
            _dashTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _dashTimer <= 0)
        {
            Dash();
        }

    }

    private void FixedUpdate()
    {
        if(_canMove)
        {
            Movement();
        }    
    }

    private void MyInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpKey) && _grounded)
        {
            Jump();
        }
    }

    private void Movement()
    {
        _moveDirection = transform.forward * _verticalInput + transform.right * _horizontalInput;
        if(_grounded)
            _rb.AddForce(_moveDirection * playerConfig.movementSpeed * 10f, ForceMode.Force);
        else
            _rb.AddForce(_moveDirection * playerConfig.movementSpeed * 10f * playerConfig.airMultiplayer, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        if(flatVel.magnitude > playerConfig.movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * playerConfig.movementSpeed;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        _rb.AddForce(transform.up * playerConfig.jumpStrength, ForceMode.Impulse);
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
        _canMove = false;
        _dashTimer = playerConfig.dashReloadTime;
        float dashTimer = playerConfig.dashDuration;
        while(dashTimer > 0)
        {
            Vector3 dashDirection = transform.TransformDirection(direction) * playerConfig.dashStrength;
            _rb.velocity = new Vector3(dashDirection.x, _rb.velocity.y, dashDirection.z);
            dashTimer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        _canMove = true;
    }
}

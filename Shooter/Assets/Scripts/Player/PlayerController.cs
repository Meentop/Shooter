using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform checkOnGroundPoint;
    [SerializeField] private PlayerConfig playerConfig;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    private Rigidbody _rb;
    private bool _grounded, _exitingSlope, _dashing;
    private float _dashTimer;
    private float _horizontalInput, _verticalInput;
    private Vector3 _moveDirection;
    private RaycastHit _slopeHit;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    private void Update()
    {
        _grounded = Physics.Raycast(transform.position, Vector3.down, 1.3f, LayerMask.GetMask("Solid"));

        MyInput();
        SpeedControl();

        if (_grounded && !_dashing)
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
        if(!_dashing)
        {
            Movement();
        }    
    }

    private void MyInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpKey) && _grounded && !_dashing)
        {
            Jump();

            Invoke(nameof(ResetJump), 1.0f);
        }
    }

    private void Movement()
    {
        _moveDirection = transform.forward * _verticalInput + transform.right * _horizontalInput;

        if (OnSlope() && !_exitingSlope)
        {
            _rb.AddForce(GetSlopeMoveDirection() * playerConfig.movementSpeed * 20f, ForceMode.Force);

            if (_moveDirection != Vector3.zero)
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        else if (_grounded)
            _rb.AddForce(_moveDirection * playerConfig.movementSpeed * 20f, ForceMode.Force);
        else if(!_grounded)
            _rb.AddForce(_moveDirection * playerConfig.movementSpeed * 20f * playerConfig.airMultiplayer, ForceMode.Force);

        _rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (!_dashing)
        {
            if (OnSlope() && !_exitingSlope)
            {
                if (_rb.velocity.magnitude > playerConfig.movementSpeed)
                    _rb.velocity = _rb.velocity.normalized * playerConfig.movementSpeed;
            }
            else
            {
                Vector3 flatVel = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
                if (flatVel.magnitude > playerConfig.movementSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * playerConfig.movementSpeed;
                    _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
                }
            }
        }
    }

    private void Jump()
    {
        _exitingSlope = true;
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        _rb.AddForce(transform.up * playerConfig.jumpStrength, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _exitingSlope = false;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out _slopeHit, 1.4f))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < playerConfig.maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;
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
        _dashing = true;
        _dashTimer = playerConfig.dashReloadTime;
        float dashTimer = playerConfig.dashDuration;
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        _rb.useGravity = false;
        while(dashTimer > 0)
        {
            Vector3 dashDirection = transform.TransformDirection(direction) * playerConfig.dashStrength;
            _rb.velocity = new Vector3(dashDirection.x, _rb.velocity.y, dashDirection.z);
            dashTimer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        _rb.useGravity = true;
        _dashing = false;
    }
}

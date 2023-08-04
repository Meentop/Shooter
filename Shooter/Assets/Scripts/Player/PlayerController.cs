using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform checkOnGroundPoint;
    private KeyCode jumpKey = KeyCode.Space;

    private Rigidbody _rb;
    private bool _grounded, _exitingSlope, _dashing;
    private float _dashTimer;
    private float _horizontalInput, _verticalInput;
    private Vector3 _moveDirection;
    private RaycastHit _slopeHit;
    private Player _player;
    private int _curDashCharges = 0;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (!Pause.pause)
        {
            _grounded = Physics.Raycast(transform.position, Vector3.down, 1.3f, LayerMask.GetMask("Solid"));

            MyInput();
            SpeedControl();

            if (_grounded && !_dashing)
                _rb.drag = _player.GetMovement().groundDrag;
            else
                _rb.drag = 0;

            if (_dashTimer < _player.GetMovement().dashReloadTime && _curDashCharges < _player.GetMovement().dashCharges)
            {
                _dashTimer += Time.deltaTime;
            }
            if( _dashTimer >= _player.GetMovement().dashReloadTime && _curDashCharges < _player.GetMovement().dashCharges)
            {
                _dashTimer = 0;
                _curDashCharges++;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && _curDashCharges > 0 && !_dashing)
            {
                Dash();
            }
            _player.SetDashInfo(_dashTimer / _player.GetMovement().dashReloadTime, _curDashCharges);
        }
    }

    private void FixedUpdate()
    {
        if (!Pause.pause && !_dashing)
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
            _rb.AddForce(20f * _player.GetMovement().movementSpeed * GetSlopeMoveDirection(), ForceMode.Force);

            if (_moveDirection != Vector3.zero)
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        else if (_grounded)
            _rb.AddForce(20f * _player.GetMovement().movementSpeed * _moveDirection, ForceMode.Force);
        else if(!_grounded)
            _rb.AddForce(20f * _player.GetMovement().airMultiplayer * _player.GetMovement().movementSpeed * _moveDirection, ForceMode.Force);

        _rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (!_dashing)
        {
            if (OnSlope() && !_exitingSlope)
            {
                if (_rb.velocity.magnitude > _player.GetMovement().movementSpeed)
                    _rb.velocity = _rb.velocity.normalized * _player.GetMovement().movementSpeed;
            }
            else
            {
                Vector3 flatVel = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
                if (flatVel.magnitude > _player.GetMovement().movementSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * _player.GetMovement().movementSpeed;
                    _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
                }
            }
        }
    }

    private void Jump()
    {
        _exitingSlope = true;
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        _rb.AddForce(transform.up * _player.GetMovement().jumpStrength, ForceMode.Impulse);
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
            return angle < _player.GetMovement().maxSlopeAngle && angle != 0;
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
        _curDashCharges--;
        _dashing = true;
        _dashTimer = 0;
        float dashTimer = _player.GetMovement().dashDuration;
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        _rb.useGravity = false;
        while (dashTimer > 0)
        {
            Vector3 dashDirection = transform.TransformDirection(direction) * _player.GetMovement().dashStrength;
            _rb.velocity = new Vector3(dashDirection.x, _rb.velocity.y, dashDirection.z);
            dashTimer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        _rb.useGravity = true;
        _dashing = false;
    }
}

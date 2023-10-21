using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPauseble
{
    [SerializeField] private Transform checkOnGroundPoint;
    private KeyCode jumpKey = KeyCode.Space;
    private KeyCode dashKey = KeyCode.LeftShift;

    private Rigidbody _rb;
    private bool _grounded, _exitingSlope, _dashing;
    private float _dashTimer;
    private float _horizontalInput, _verticalInput;
    private Vector3 _moveDirection;
    private RaycastHit _slopeHit;
    private Player _player;
    private int _curDashCharges = 0;
    private Vector3 prepauseVelocity;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _player = GetComponent<Player>();
        PauseManager.OnSetPause += OnSetPause;
    }

    private void Update()
    {
        if (!PauseManager.Pause)
        {
            _grounded = Physics.Raycast(transform.position, Vector3.down, 1.3f, LayerMask.GetMask("Solid"));

            MyInput();
            SpeedControl(_dashing ? _player.Characteristics.dashStrength : _player.Characteristics.movementSpeed);

            if (_grounded && !_dashing)
                _rb.drag = _player.Characteristics.groundDrag;
            else
                _rb.drag = 0;

            if (_dashTimer < _player.Characteristics.dashReloadTime && _curDashCharges < _player.Characteristics.dashCharges)
            {
                _dashTimer += Time.deltaTime;
            }
            if ( _dashTimer >= _player.Characteristics.dashReloadTime && _curDashCharges < _player.Characteristics.dashCharges)
            {
                _dashTimer = 0;
                _curDashCharges++;
            }
            
            _player.SetDashInfo(_dashTimer / _player.Characteristics.dashReloadTime, _curDashCharges);
        }
    }

    private void FixedUpdate()
    {
        if (!PauseManager.Pause)
        {
            if (!_dashing) 
            {
                _moveDirection = transform.forward * _verticalInput + transform.right * _horizontalInput;
                Movement(_moveDirection, _player.Characteristics.movementSpeed);
            }
            else
                Movement(_moveDirection, _player.Characteristics.dashStrength);
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

        if (Input.GetKeyDown(dashKey) && _curDashCharges > 0 && !_dashing)
        {
            Dash();
        }
    }

    private void Movement(Vector3 direction, float speed)
    {
        if (OnSlope() && !_exitingSlope)
        {
            _rb.AddForce(20f * speed * GetSlopeMoveDirection(direction), ForceMode.Force);

            if (direction != Vector3.zero)
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        else if (_grounded || _dashing)
        {
            _rb.AddForce(20f * speed * direction, ForceMode.Force);
        }
        else if (!_grounded)
        {
            _rb.AddForce(20f * _player.Characteristics.airMultiplayer * speed * direction, ForceMode.Force);
        }

        _rb.useGravity = !OnSlope();
    }

    private void SpeedControl(float speed)
    {
        if (OnSlope() && !_exitingSlope)
        {
            if (_rb.velocity.magnitude > speed)
                _rb.velocity = _rb.velocity.normalized * speed;
        }
        else
        {
            Vector3 flatVel = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            if (flatVel.magnitude > speed)
            {
                Vector3 limitedVel = flatVel.normalized * speed;
                _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        _exitingSlope = true;
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        _rb.AddForce(transform.up * _player.Characteristics.jumpStrength, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, 1.4f))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _player.Characteristics.maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, _slopeHit.normal).normalized;
    }

    private void Dash()
    {
        StartCoroutine(DashCor());
    }

    private IEnumerator DashCor()
    {
        _curDashCharges--;
        _dashing = true;
        _dashTimer = 0;
        float dashTimer = _player.Characteristics.dashDuration;
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        _rb.useGravity = false;

        _moveDirection = transform.forward * _verticalInput + transform.right * _horizontalInput;
        if (_moveDirection == Vector3.zero)
            _moveDirection = transform.forward;

        while (dashTimer > 0)
        {
            if (!PauseManager.Pause)
                dashTimer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        _rb.useGravity = true;
        _dashing = false;
    }

    public void OnSetPause(bool value)
    {
        if (value)
        {
            prepauseVelocity = _rb.velocity;
            _rb.isKinematic = true;
        }
        else
        {
            _rb.isKinematic = false;
            _rb.velocity = prepauseVelocity;
        }
    }

    private void OnDestroy()
    {
        PauseManager.OnSetPause -= OnSetPause;
    }
}

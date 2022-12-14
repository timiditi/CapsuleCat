using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerControls PlayerController;

    public Transform pivot;
    public Transform mainBody;
    
    public float maxSpeed = 50;
    public float stoppingSpeed = 150;

    public float jumpTime = 1;
    public float jumpSpeed = 3;
    public float weight = 1;
    public float jumpDecel = 10;
    private static float _gravityConstant = -9.81f;
    private bool _isGrounded;

    private float _lastGroundedY;
    private float _yVelocity;
    private float _airTime;

    private float _currentVelocity;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        PlayerController = new PlayerControls();
        _lastGroundedY = mainBody.position.y;
        _isGrounded = true;
    }

    private void OnEnable()
    {
        PlayerController.Enable();
    }
    
    private void OnDisable()
    {
        PlayerController.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        float movement = PlayerController.Land.MoveLR.ReadValue<float>();
        if (movement != 0)
        {
            _currentVelocity = maxSpeed * movement;
            pivot.Rotate(new Vector3(0, 1, 0), -1 * _currentVelocity * Time.deltaTime);
        }
        else if (Math.Abs(_currentVelocity) > float.Epsilon)
        {
            _currentVelocity =  _currentVelocity > 0 ? Math.Max(_currentVelocity - stoppingSpeed * Time.deltaTime, 0) 
                                                     : Math.Min(_currentVelocity + stoppingSpeed * Time.deltaTime, 0);
            pivot.Rotate(new Vector3(0, 1, 0), -1 * _currentVelocity * Time.deltaTime);
        }
        else
        {
            _currentVelocity = 0.0f;
        }

        if (_isGrounded && PlayerController.Land.Jump.triggered)
        {
            _isGrounded = false;
            _lastGroundedY = mainBody.position.y;
            _yVelocity = jumpSpeed;
            _airTime = 0;
        }

        if (!_isGrounded)
        {
            _airTime += Time.deltaTime;
            if (_airTime > jumpTime)
            {
                if (_yVelocity > 0.0f)
                {
                    _yVelocity += _gravityConstant * jumpDecel * weight * Time.deltaTime;
                    if (_yVelocity < 0.0f) _yVelocity = 0.0f;
                }
                else
                {
                    _yVelocity += _gravityConstant * weight * Time.deltaTime;
                }
            }
            
            mainBody.position += new Vector3(0, _yVelocity, 0) * Time.deltaTime;
            if (mainBody.position.y <= _lastGroundedY)
            {
                var position = mainBody.position;
                position = new Vector3(position.x, _lastGroundedY, position.z);
                mainBody.position = position;
                _yVelocity = 0.0f;
                _isGrounded = true;
            }
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MoveState
{
    Walk, Run,
}

public class PlayerController : MonoBehaviour, IAttackable
{
    [Header("이동 관련 스테이터스")]
    public float walkSpeed = 3.0f;
    public float runSpeed = 5.0f;
    public float jumpPower = 5.0f;
    public float turnSpeed = 0.3f;

    [Header("플레이어 설정")]
    public int maxHP = 100;
    public int currentHP;
    public float attackPower = 5;
    public GameObject weapon;
    
    private float _moveSpeed = 0.0f;
    private float _gravity = -9.8f;
    private bool _isRunning = false;
    private bool _isButtonDown = false;
    
    private Vector3 _inputDir;
    private Quaternion _targetRotation = Quaternion.identity;
    private Animator _animator;
    private CharacterController _controller;
    private MoveState _moveState = MoveState.Walk;
    
    private static readonly int Movement = Animator.StringToHash("movement");


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        currentHP = maxHP;
        _moveSpeed = walkSpeed;
    }

    private void Update()
    {
        if (_inputDir.magnitude > 0.0f)
        {
            weapon.SetActive(true);
            if (_moveState == MoveState.Walk)
            {
                _animator.SetFloat(Movement, 0.5f);
            }
            else if (_moveState == MoveState.Run)
            {
                _animator.SetFloat(Movement, 1);
            }
        }
        else
        {
            weapon.SetActive(false);
            _animator.SetFloat(Movement, 0);
        }
        
        _controller.SimpleMove(_inputDir * _moveSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, turnSpeed);
       // if (!_controller.isGrounded)
            
        // if (_inputDir.magnitude > 0)
        //     transform.rotation = Quaternion.Lerp(transform.rotation, GetCamAxisInputDir(), turnSpeed);
    }

    // private Quaternion GetCamAxisInputDir() => Quaternion.LookRotation(_inputDir) * GetCamAxisDir();
    //
    // private Quaternion GetCamAxisDir()
    // {
    //     Debug.Log(Camera.main.transform.eulerAngles.y);
    //     return Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
    // }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        _inputDir.x = callbackContext.ReadValue<Vector2>().x;
        _inputDir.z = callbackContext.ReadValue<Vector2>().y;
        
        if (_inputDir.sqrMagnitude > 0)
        {
            _inputDir = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0) * _inputDir;
            
            _targetRotation = Quaternion.LookRotation(_inputDir); 
        }
    }

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        if (_controller.isGrounded)
        {
            _controller.SimpleMove(Vector3.up * jumpPower);
        }
    }

    public void OnDash(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            _isRunning = true;
            _moveSpeed = runSpeed;
        }

        if (callbackContext.canceled)
        {
            _isRunning = false;
            _moveSpeed = walkSpeed;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
    }
}

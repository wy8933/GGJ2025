using ObjectPoolings;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.DirectorControlPlayable;

[RequireComponent(typeof(PlayerInput), typeof(PlayerController))]
public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    public PlayerInput playerInput;

    public Animator playerAnimator;

    [Header("Input Action References")]
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _fireAction;
    [SerializeField] private InputActionReference _pauseAction;

    private void Start()
    {
        _player = GetComponent<PlayerController>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _moveAction.action.Enable();
        _fireAction.action.Enable();
        _pauseAction.action.Enable();

        _moveAction.action.performed += OnMovePerformed;
        _moveAction.action.canceled += OnMoveCanceled;

        _fireAction.action.performed += OnAttackPerformed;
        _fireAction.action.canceled += OnAttackCanceled;

        _pauseAction.action.performed += OnPausePerformed;
    }

    private void OnDisable()
    {
        _moveAction.action.Disable();
        _fireAction.action.Disable();
        _pauseAction.action.Disable();

        _moveAction.action.performed -= OnMovePerformed;
        _moveAction.action.canceled -= OnMoveCanceled;

        _fireAction.action.performed -= OnAttackPerformed;
        _fireAction.action.canceled -= OnAttackCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _player.moveDirection = context.ReadValue<Vector2>().normalized;
        playerAnimator.SetBool("IsWalking", true);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _player.moveDirection = context.ReadValue<Vector2>().normalized;
        playerAnimator.SetBool("IsWalking", false);
    }

    private void OnAttackPerformed(InputAction.CallbackContext context) {
        if (_player.weaponType == WeaponType.ShotGun)
        {
            _player.Attack();
        }
        else
        {
            _player.isShooting = true;
        }
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        _player.isShooting = false;
    }

    private void OnPausePerformed(InputAction.CallbackContext context) { 
        GameManager.Instance.Pause();
    }
}



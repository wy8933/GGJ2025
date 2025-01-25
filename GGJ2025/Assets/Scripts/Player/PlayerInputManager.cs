using ObjectPoolings;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(PlayerController))]
public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    public PlayerInput playerInput;

    [Header("Input Action References")]
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _fireAction;

    private void Start()
    {
        _player = GetComponent<PlayerController>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _moveAction.action.Enable();
        _fireAction.action.Enable();

        _moveAction.action.performed += OnMove;
        _moveAction.action.canceled += OnMove;


        _fireAction.action.performed += OnAttack;
    }

    private void OnDisable()
    {
        _moveAction.action.Disable();
        _fireAction.action.Disable();

        _moveAction.action.performed -= OnMove;
        _moveAction.action.canceled -= OnMove;

        _fireAction.action.performed -= OnAttack;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _player.moveDirection = context.ReadValue<Vector2>().normalized;
    }

    private void OnAttack(InputAction.CallbackContext context) {
        _player.Attack();
    }
}



using BulletPro;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputMode
{
    None,
    Gameplay,
    Dialogue,
    Loading,
    Hamiltonian
}
public class PlayerController : MonoBehaviour
{
    public Vector2 MovementVector { get; private set; }
    private Vector2 _cachedMovementVector;
    private bool _movementWasBlockedLastFrame = false;
    private Player _player;
    private PlayerInput _playerInput;

    // Start is called before the first frame update
    void Awake()
    {
        _player = GetComponent<Player>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            UIManager.Instance.ChangeLiquidAmount(50f);
        }

        if (_movementWasBlockedLastFrame && !_player.ActionSM.CurrentState.BlockMovement)
        {
            _movementWasBlockedLastFrame = false;
            MovementVector = _player.OnMovementUnblocked(_cachedMovementVector);
        }
    }

    public void SwitchActionMap(InputMode mode)
    {
        string actionMap = null;
        switch(mode)
        {
            case InputMode.Gameplay:
                actionMap = "Gameplay";
                break;
            case InputMode.Dialogue:
                actionMap = "Dialogue";
                break;
            case InputMode.Loading:
                actionMap = "Loading";
                break;
            case InputMode.Hamiltonian:
                actionMap = "Hamiltonian";
                break;
            default:
                actionMap = "Gameplay";
                break;
        }

        _playerInput.SwitchCurrentActionMap(actionMap);
    }
    public void MoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _cachedMovementVector = input;

        bool movementBlocked = _player.ActionSM.CurrentState.BlockMovement;

        if (movementBlocked)
        {
            _movementWasBlockedLastFrame = true;
            MovementVector = Vector2.zero;
            return;
        }

        MovementVector = input;

        if (_player.ActionSM.CurrentState == _player.GooState)
            return;

        if (input != Vector2.zero)
            _player.StartMovement();
        else
            _player.StopMovement();
    }
    public void FireInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _player.StartShooting();
        }
        else if (context.canceled)
        {
            _player.StopShooting();
        }
    }
    public void GooInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _player.StartGoo();
        }
        else if(context.canceled)
        {
            _player.StopGoo();
        }
    }
    public void SuctionInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _player.StartSuction();
        }
    }

    public void InteractInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            _player.StartInteraction();
        }
    }

    public void ContinueInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log("Continue Input");
            _player.ContinueChat();
        }
    }

    public void Aim(InputAction.CallbackContext context)
    {
        Vector2 aimDirection = context.ReadValue<Vector2>();
        _player.Aim(aimDirection);

    }
}

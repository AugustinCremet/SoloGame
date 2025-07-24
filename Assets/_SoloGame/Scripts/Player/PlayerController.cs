using BulletPro;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 MovementVector { get; private set; }
    private Vector2 _cachedMovementVector;
    private bool _movementWasBlockedLastFrame = false;
    private Player _player;

    public static event Action OnInteract;
    public static event Action OnSuction;


    // Start is called before the first frame update
    void Awake()
    {
        _player = GetComponent<Player>();
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
            OnSuction?.Invoke();
        }

        if (_movementWasBlockedLastFrame && !_player.StateMachine.CurrentState.BlockMovement)
        {
            _movementWasBlockedLastFrame = false;
            MovementVector = _player.OnMovementUnblocked(_cachedMovementVector);
        }
    }
    public void MoveInput(InputAction.CallbackContext context)
    {
        _cachedMovementVector = context.ReadValue<Vector2>();

        bool movementBlocked = _player.StateMachine.CurrentState.BlockMovement;
        
        if (!movementBlocked)
        {
            MovementVector = context.ReadValue<Vector2>();
            if (_player.StateMachine.CurrentState == _player.GooState)
                return;

            if (MovementVector.sqrMagnitude > 0.01f)
            {
                _player.StartMovement();
            }
            else
            {
                _player.StopMovement();
            }       
        }
        else
        {
            _movementWasBlockedLastFrame = true;
            MovementVector = Vector2.zero;
        }
    }
    public void FireInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _player.StartShooting();
        }
        else if (context.canceled)
        {
            Debug.Log("Cancelled");
            _player.StopShooting();
        }
    }
    public void DashInput(InputAction.CallbackContext context)
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

    public void InteractInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OnInteract?.Invoke();
        }
    }


}

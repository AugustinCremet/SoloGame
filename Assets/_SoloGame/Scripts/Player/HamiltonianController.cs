using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;


public class HamiltonianController : MonoBehaviour
{
    private PlayerControls _controls;
    [SerializeField] Tilemap _pathTilemap;
    [SerializeField] Tilemap _obstacleTilemap;
    [SerializeField] CardinalPoint _startingDirection;
    private Vector3 _startingPosition;

    private void Awake()
    {
        _startingPosition = transform.position;
        _controls = new PlayerControls();
    }

    private void Start()
    {
        Vector3Int gridPosition = _pathTilemap.WorldToCell(transform.position);
        HamiltonianLogic.Instance.SetCurrentTile(gridPosition, _startingDirection);
    }

    private void OnEnable()
    {
        _controls.Enable();
        _controls.Hamiltonian.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
        _controls.Hamiltonian.Restart.performed += ctx => Restart();                        
    }


    private void OnDisable()
    {
        _controls.Hamiltonian.Movement.performed -= ctx => Move(ctx.ReadValue<Vector2>());
        _controls.Disable();
    }

    private void Move(Vector2 direction)
    {
        Vector3Int oldGridPosition = _pathTilemap.WorldToCell(transform.position);
        Vector3Int newGridPosition = _pathTilemap.WorldToCell(transform.position + (Vector3)direction);

        if(CanMove(newGridPosition) && !HamiltonianLogic.Instance.WasTileVisited(newGridPosition))
        {
            HamiltonianLogic.Instance.HandleMovementChanges(oldGridPosition, newGridPosition, direction);

            transform.position += (Vector3)direction;
        }
    }

    private void Restart()
    {
        HamiltonianLogic.Instance.Restart();
        transform.position = _startingPosition;
    }

    private bool CanMove(Vector3Int newGridPosition)
    {
        if(!_pathTilemap.HasTile(newGridPosition) || _obstacleTilemap.HasTile(newGridPosition))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

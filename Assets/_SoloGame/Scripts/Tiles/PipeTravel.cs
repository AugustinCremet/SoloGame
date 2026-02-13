using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PipeTravel : MonoBehaviour, IUniqueIdentifier
{
    [SerializeField] protected string _uniqueID;
    public string UniqueID { get => _uniqueID; set => _uniqueID = value; }

    private Tilemap _pipe;
    [SerializeField] private List<Transform> _pipePath = new List<Transform>();
    [SerializeField] private Transform _entranceA;
    [SerializeField] private Transform _entranceB;
    [SerializeField] private float _speed = 1.0f;

    private Vector3 _endPoint;

    private void Awake()
    {
        _pipe = GetComponent<Tilemap>();
    }
    public void EnterPipe(Vector3 startingPos, PipeSide startingSide, Player player, PlayerController controller)
    {
        //Vector3Int startingCellPos = _pipe.WorldToCell(startingPos);

        //if(_pipe.HasTile(startingCellPos))
        {
            controller.SwitchActionMap(InputMode.Loading);
            player.transform.position = startingPos;
            player.SetInvinsibility(true);
            player.DisableCollision();
            if(startingSide == PipeSide.A)
            {
                _endPoint = _entranceB.position;
                StartCoroutine(MoveInPipe(player, controller));
            }
            else
            {
                _endPoint = _entranceA.position;
                StartCoroutine(MoveInPipeReverse(player, controller));
            }
        }
    }

    private IEnumerator MoveInPipeReverse(Player player, PlayerController controller)
    {
        if(_pipePath.Count > 0)
        {
            for (int i = _pipePath.Count; i != 0; i--)
            {
                yield return MoveToPoint(player, _pipePath[i - 1].position);
            }
        }
        
        yield return MoveToPoint(player, _endPoint);

        MySceneManager.Instance.SetCheckpoint(player.transform.position);
        player.SetInvinsibility(false);
        player.EnableCollision();
        controller.SwitchActionMap(InputMode.Gameplay);
    }

    private IEnumerator MoveInPipe(Player player, PlayerController controller)
    {
        if (_pipePath.Count > 0)
        {
            for(int i = 0 ; i < _pipePath.Count; i++)
            {
                yield return MoveToPoint(player, _pipePath[i].position);
            }
        }

        yield return MoveToPoint(player, _endPoint);

        MySceneManager.Instance.SetCheckpoint(player.transform.position);
        player.SetInvinsibility(false);
        player.EnableCollision();
        controller.SwitchActionMap(InputMode.Gameplay);
    }

    private IEnumerator MoveToPoint(Player player, Vector3 target)
    {
        while ((player.transform.position - target).sqrMagnitude > 0.001f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, target, _speed * Time.deltaTime);
            yield return null;
        }
    }
}

    

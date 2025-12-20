using UnityEngine;

public enum ELiquidType { None, Blue, Red }
public class Bucket : MonoBehaviour, IInteractable
{
    public enum EBucketType { Source, Receiver }

    [SerializeField] EBucketType _bucketType;
    [SerializeField] Sprite _filledBucket;
    [SerializeField] ELiquidType _liquidType;
    [SerializeField] string[] _phrase1;
    [SerializeField] string[] _phrase2;

    private Player _player;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Interact(Player player, PlayerController controller)
    {
        if (_player == null)
        {
            _player = player;
        }
        switch(_bucketType)
        {
            case EBucketType.Source:
                SourceInteraction();
                break;
            case EBucketType.Receiver:
                ReceiverInteraction();
                break;
        }
    }

    private void ReceiverInteraction()
    {
        if(_player.CurrentLiquid == _liquidType)
        {
            _player.StartChat(_phrase2, () => PourInto());
        }
        else
        {
            _player.StartChat(_phrase1 );
        }
    }

    private void SourceInteraction()
    {
        if(_player.HasAbility(EPlayerSkill.Goo))
        {
            _player.StartChat(_phrase2, () => ReceiveFrom());
        }
        else
        {
            _player.StartChat(_phrase1 );
        }
    }
    private void PourInto()
    {
        _spriteRenderer.sprite = _filledBucket;
        _player.RemoveLiquid();
        UIManager.Instance.ChangeLiquidAmount(0f);
    }

    private void ReceiveFrom()
    {
        UIManager.Instance.ChangeLiquidAmount(100f);
        _player.GiveLiquid(_liquidType);
    }

    public void ShowPopup()
    {
        throw new System.NotImplementedException();
    }

    public void HidePopup()
    {
        throw new System.NotImplementedException();
    }
}

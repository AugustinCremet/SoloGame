using TMPro;
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
    [SerializeField] bool _containsKey;
    [SerializeField] Item _itemPrefab;
    [SerializeField] string[] _phraseForKey;

    private bool doOnce = true;

    private Player _player;
    private SpriteRenderer _spriteRenderer;
    private GameObject _popup;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _popup = GetComponentInChildren<TextMeshProUGUI>().transform.gameObject;
        _popup.SetActive(false);
    }

    public void Interact(Player player, PlayerController controller)
    {
        if (_player == null)
        {
            _player = player;
        }
        if(_containsKey && doOnce)
        {
            doOnce = false;
            _player.StartChat(_phraseForKey, () => GiveReward());

            return;
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

    private void GiveReward()
    {
        Item item = Instantiate(_itemPrefab);
        item.transform.position = _player.transform.position + Vector3.up * 1.5f;
        item.PlayRewardAnimation(_player);
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
        _popup.SetActive(true);
    }

    public void HidePopup()
    {
        _popup.SetActive(false);
    }
}

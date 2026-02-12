using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour, ICollectable, IUniqueIdentifier
{
    [SerializeField] private SOItem _itemData;
    [SerializeField] private string _uniqueID;

    public string UniqueID { get => _uniqueID; set => _uniqueID = value; }

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_itemData != null && _itemData.Sprite != null)
        {
            _spriteRenderer.sprite = _itemData.Sprite;
        }
    }

    private void Start()
    {
        if (GameManager.Instance.IsCollectableCollected(UniqueID))
        {
            Destroy(gameObject);
        }
    }

    public void PlayRewardAnimation(Player player)
    {
        StartCoroutine(RewardCR(player));
    }

    private IEnumerator RewardCR(Player player)
    {
        player.GetComponent<PlayerController>().SwitchActionMap(InputMode.Dialogue);
        yield return new WaitForSeconds(1f);

        OnCollect(player);
        player.GetComponent<PlayerController>().SwitchActionMap(InputMode.Gameplay);
    }

    public void OnCollect(Player player)
    {
        if (_itemData != null)
        {
            _itemData.GiveToPlayer(player);
            GameManager.Instance.MarkCollectableColledted(UniqueID);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError($"Item {gameObject.name} has no SOItem data assigned!");
        }
    }
}

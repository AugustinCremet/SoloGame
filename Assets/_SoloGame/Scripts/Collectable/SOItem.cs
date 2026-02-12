using UnityEngine;

public enum EItemType
{
    Key,
    HealthPotion,
    PowerUp,
    Ability,
    Consumable
}

[CreateAssetMenu(fileName = "Item", menuName = "Item/Collectable")]
public class SOItem : ScriptableObject
{
    [Header("Visual")]
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _itemName;
    [TextArea(3, 5)]
    [SerializeField] private string _description;

    [Header("Item Properties")]
    [SerializeField] private EItemType _itemType;
    [SerializeField] private int _healAmount;
    [SerializeField] private EPlayerSkill _abilityToGrant;

    public Sprite Sprite => _sprite;
    public string ItemName => _itemName;
    public string Description => _description;
    public EItemType ItemType => _itemType;
    public int HealAmount => _healAmount;
    public EPlayerSkill AbilityToGrant => _abilityToGrant;

    public void GiveToPlayer(Player player)
    {
        switch (_itemType)
        {
            case EItemType.Key:
                player.GiveKey();
                break;

            case EItemType.HealthPotion:
                player.Heal(_healAmount);
                break;

            case EItemType.PowerUp:
                // TODO add method
                break;

            case EItemType.Ability:
                player.GrantAbility(_abilityToGrant);
                break;

            case EItemType.Consumable:
                // TODO add method
                break;

            default:
                Debug.LogWarning($"Item type {_itemType} not handled!");
                break;
        }
    }
}

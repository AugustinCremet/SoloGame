using UnityEngine;

public class Door : MonoBehaviour, IOpenable
{
    [SerializeField] Sprite _openSprite;
    [SerializeField] Sprite _closeSprite;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }
    public void TryOpen()
    {
        _spriteRenderer.sprite = _openSprite;
        _boxCollider.enabled = false;
    }
}

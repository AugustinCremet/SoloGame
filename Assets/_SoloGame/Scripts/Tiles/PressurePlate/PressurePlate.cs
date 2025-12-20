using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] Sprite _unpressedPlate;
    [SerializeField] Sprite _pressedPlate;
    private SpriteRenderer _spriteRenderer;
    public bool IsPressed {  get; private set; }
    public bool StayPressed { get; private set; } = true;
    public Action<PressurePlate, bool> OnPlateChanged;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _unpressedPlate;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.TryGetComponent(out Player player))
        {
            IsPressed = true;
            _spriteRenderer.sprite = _pressedPlate;
            OnPlateChanged?.Invoke(this, IsPressed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            if(!StayPressed)
            {
                IsPressed = false;
                OnPlateChanged?.Invoke(this, IsPressed);
            }
            else
            {

            }
        }
    }

    public void ResetPlate()
    {
        _spriteRenderer.sprite = _unpressedPlate;
        IsPressed = false;
    }
}

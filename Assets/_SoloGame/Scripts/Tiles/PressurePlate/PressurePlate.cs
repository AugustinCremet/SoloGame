using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Sprite _unpressedPlate;
    [SerializeField] private Sprite _pressedPlate;
    [SerializeField] private bool _stayPressed = false;
    [SerializeField] private bool _canPlayerPress = false;
    private SpriteRenderer _spriteRenderer;
    public bool IsPressed {  get; private set; }
    public Action<PressurePlate, bool> OnPlateChanged;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _unpressedPlate;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (!_canPlayerPress)
        {
            if(collision.CompareTag("PlayerFeet"))
            {
                return;
            }
        }

        IsPressed = true;
        _spriteRenderer.sprite = _pressedPlate;
        OnPlateChanged?.Invoke(this, IsPressed);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player) || collision.TryGetComponent(out PushBlock block))
        {
            if(!_stayPressed)
            {
                IsPressed = false;
                _spriteRenderer.sprite = _unpressedPlate;
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

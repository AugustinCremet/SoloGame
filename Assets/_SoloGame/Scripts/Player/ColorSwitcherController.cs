using UnityEngine;
using UnityEngine.InputSystem;

public class ColorSwitcherController : MonoBehaviour
{
    Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }
    public void NextColor(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            int nextIndex = _player.AvailableRainbowColors.IndexOf(_player.CurrentRainbowColor) + 1;
            SwitchColor(nextIndex);
        }
    }

    public void SwitchColor(int nextIndex)
    {
        if (nextIndex > _player.AvailableRainbowColors.Count - 1)
        {
            _player.ChangeColor(_player.AvailableRainbowColors[0]);
        }
        else
        {
            _player.ChangeColor(_player.AvailableRainbowColors[nextIndex]);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class ColorSwitcherController : MonoBehaviour
{
    public static event Action<bool> onNextColorSwitch;

    public void NextColor(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onNextColorSwitch?.Invoke(true);
        }
    }

    public void PreviousColor(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onNextColorSwitch?.Invoke(false);
        }
    }
}

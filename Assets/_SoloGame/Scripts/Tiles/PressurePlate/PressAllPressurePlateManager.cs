using System.Threading;
using UnityEngine;

public class PressAllPressurePlateManager : PressurePlateManagerBase
{
    protected override void HandlePlateChanged(PressurePlate pressurePlate, bool pressed)
    {
        foreach(var plate in _plates)
        {
            if (!plate.IsPressed)
                return;
        }

        foreach(var go in _affectedGO)
        {
            go.GetComponent<IOpenable>().TryOpen();
        }
    }
}

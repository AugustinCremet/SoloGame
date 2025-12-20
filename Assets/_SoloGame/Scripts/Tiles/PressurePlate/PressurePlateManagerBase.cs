using UnityEngine;

public abstract class PressurePlateManagerBase : MonoBehaviour
{
    [SerializeField] protected PressurePlate[] _plates;
    [SerializeField] protected GameObject[] _affectedGO;

    protected virtual void Awake()
    {
        foreach(PressurePlate plate in _plates)
        {
            plate.OnPlateChanged += HandlePlateChanged;
        }
    }

    protected abstract void HandlePlateChanged(PressurePlate plate, bool pressed);
}

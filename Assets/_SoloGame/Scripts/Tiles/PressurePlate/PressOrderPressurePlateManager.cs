using UnityEngine;

public class PressOrderPressurePlateManager : PressurePlateManagerBase
{
    private int _currentPlatePressed = 0;
    private int _rightPlatePressed = 0;
    private bool _isSequenceCompleted = false;
    protected override void HandlePlateChanged(PressurePlate plate, bool pressed)
    {
        if(_isSequenceCompleted)
        {
            return;
        }

        if (_plates[_currentPlatePressed] == plate)
        {
            _rightPlatePressed++;

            if(_rightPlatePressed >= _plates.Length)
            {
                foreach (var go in _affectedGO)
                {
                    go.GetComponent<IOpenable>().TryOpen();
                }

                _isSequenceCompleted = true;
            }
        }

        _currentPlatePressed++;
        if(!_isSequenceCompleted && _currentPlatePressed >= _plates.Length)
        {
            foreach (var pl in _plates)
            {
                pl.ResetPlate();
            }
            _rightPlatePressed = 0;
            _currentPlatePressed = 0;
        }
    }
}

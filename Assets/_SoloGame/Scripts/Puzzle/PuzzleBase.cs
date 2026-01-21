using System.Linq;
using UnityEngine;

public abstract class PuzzleBase : MonoBehaviour, IUniqueIdentifier, IState
{
    [SerializeField] private string _uniqueID;
    protected bool _isCompleted = false;

    public string UniqueID { get => _uniqueID; set => _uniqueID = value; }

    protected abstract bool CheckCompletion();
    protected abstract void OnPuzzleCompleted();

    protected void TryComplete()
    {
        if(_isCompleted)
            return;

        if(!CheckCompletion())
            return;

        _isCompleted = true;
        OnPuzzleCompleted();

        FloorStateManager.Instance.SaveFloor();
        GameManager.Instance.MarkPuzzleCleared(_uniqueID);
    }

    public virtual void SaveState(GameState state)
    {
        if(_isCompleted && !state.CompletedPuzzles.Contains(_uniqueID))
            state.CompletedPuzzles.Add(_uniqueID);
    }

    public virtual void ResetState() { }

    public void LoadState(GameState state)
    {
        
    }
}

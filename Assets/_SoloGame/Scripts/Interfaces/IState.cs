using UnityEngine;

public interface IState
{
    void LoadState(GameState state);
    void SaveState(GameState state);
    void ResetState();
}

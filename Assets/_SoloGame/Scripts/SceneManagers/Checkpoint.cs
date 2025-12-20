using UnityEngine;

public class Checkpoint
{
    public Vector3 Position { get; private set; }

    public Checkpoint(Vector3 position)
    {
        Position = position;
    }
}

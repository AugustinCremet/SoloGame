using UnityEngine;

[System.Serializable]
public class Cooldown
{
    public float Duration {  get; private set; }
    private float _lastUseTime = -1f;
    public bool IsReady => Time.time >= _lastUseTime + Duration;
    public float RemainingTime => Mathf.Max(0, (_lastUseTime + Duration) - Time.time);

    public Cooldown(float duration)
    {
        Duration = duration;
    }

    public void Use()
    {
        _lastUseTime = Time.time;
    }
}

using UnityEngine;

public class Liquid : MonoBehaviour
{
    private static readonly System.Collections.Generic.List<Liquid> _allLiquids = new();
    private void OnEnable() => _allLiquids.Add(this);
    private void OnDisable() => _allLiquids.Remove(this);

    public bool IsPlayerInside;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player")) return;

        foreach (var liquid in _allLiquids)
        {
            if (liquid == this) continue;
            if (liquid.IsPlayerInside)
            {
                IsPlayerInside = true;
                return;
            }
        }

        IsPlayerInside = true;
        Debug.Log("Enter bad liquid");
        UIManager.Instance.RemoveLiquidPeriodicly(collision.GetComponent<Player>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Check if the player is still inside another liquid before calling exit logic
        bool insideAnother = false;
        foreach (var liquid in _allLiquids)
        {
            if (liquid == this) continue;
            if (liquid.IsPlayerInside)
            {
                insideAnother = true;
                break;
            }
        }

        IsPlayerInside = false;

        if (!insideAnother)
        {
            Debug.Log("Exit bad liquid");
            UIManager.Instance.StopRemovingLiquid();
        }
    }
}

using UnityEngine;

public static class UtilityFunctions
{
    public static bool IsPlayerInSight(Vector3 startingPosition, Transform playerTransform)
    {
        bool isInSight = false;
        int _layerMask = 1 << playerTransform.gameObject.layer;
        _layerMask |= 1 << 9;

        RaycastHit2D hit = Physics2D.Raycast(startingPosition, playerTransform.position - startingPosition, Mathf.Infinity, _layerMask);

        if (hit.collider != null)
        {
            bool hasLineOfSight = hit.collider.CompareTag("Player");
            isInSight = hasLineOfSight;
        }

        return isInSight;
    }
}

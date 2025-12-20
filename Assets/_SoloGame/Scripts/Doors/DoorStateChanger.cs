using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStateChanger : MonoBehaviour
{
    [SerializeField] Collider2D Collider2D;

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");
    }
}

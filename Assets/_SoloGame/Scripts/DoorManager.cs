using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] GameObject DoorToOpen;

    private void Start()
    {
        Destroy(DoorToOpen);
    }
}

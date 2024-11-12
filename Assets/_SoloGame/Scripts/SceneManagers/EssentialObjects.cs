using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialObjects : MonoBehaviour
{
    [SerializeField] GameObject _essentialObjectsPrefab;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

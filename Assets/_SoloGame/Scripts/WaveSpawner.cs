using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] string _uniqueID;
    [SerializeField] List<Wave> _waves = new List<Wave>();
    [SerializeField] TilemapRenderer _closingDoors;
    [SerializeField] TilemapCollider2D _closingCollider;

    private bool _startFirstWave = false;

    private void Awake()
    {
        if(string.IsNullOrEmpty(_uniqueID))
        {
            _uniqueID = System.Guid.NewGuid().ToString();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(CloseDoor());
            SpawnWave(0);
        }
    }

    IEnumerator CloseDoor()
    {
        _closingDoors.enabled = true;
        _closingCollider.enabled = true;
        yield return null;
    }

    private void SpawnWave(int waveIndex)
    {
        Debug.Log("Spawn");
        if(waveIndex >= 0 && waveIndex < _waves.Count)
        {
            Wave wave = _waves[waveIndex];
            foreach(var enemy in wave.enemies)
            {
                Debug.Log("Spawn enemy");
                Instantiate(enemy.prefab, enemy.position);
            }
        }
    }
}

[System.Serializable]
public class Wave
{
    public List<EnemySpawnInfo> enemies = new List<EnemySpawnInfo>();
}

[System.Serializable]
public struct EnemySpawnInfo
{
    public GameObject prefab;
    public Transform position;
}

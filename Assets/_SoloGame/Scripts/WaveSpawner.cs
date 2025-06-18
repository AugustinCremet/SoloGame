using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] string _uniqueID;
    [SerializeField] List<Wave> _waves = new List<Wave>();
    [SerializeField] GameObject _spawnWarning;
    [SerializeField] GameObject _closingPrefab;
    private List<GameObject> _closingPrefabChildren = new List<GameObject>();
    private List<Enemy> _aliveEnemies = new List<Enemy>();
    private int _currentWave = 0;

    private void Awake()
    {
        foreach(Transform child in _closingPrefab.transform)
        {
            _closingPrefabChildren.Add(child.gameObject);
        }
    }

    [ContextMenu("Generate Unique ID")]
    private void GenerateUniqueID()
    {
        _uniqueID = System.Guid.NewGuid().ToString();
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDeath += OnEnemyDeath;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= OnEnemyDeath;
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        if (!_aliveEnemies.Remove(enemy))
            return;

        if(_aliveEnemies.Count <= 0)
        {
            if(_currentWave >= _waves.Count)
            {
                Debug.Log("All waves cleared");
                GameManager.Instance.MarkWaveCleared(_uniqueID);
                StartCoroutine(ChangeDoorState(false));
            }
            SpawnWave(_currentWave);
            _currentWave++;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player") && !GameManager.Instance.IsWaveCleared(_uniqueID))
        {
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(ChangeDoorState(true));
            SpawnWave(_currentWave);
            _currentWave++;
        }
    }

    IEnumerator ChangeDoorState(bool isActive)
    {
        foreach(GameObject child in _closingPrefabChildren)
        {
            child.SetActive(isActive);
        }
        yield return null;
    }

    private void SpawnWave(int waveIndex)
    {
        if(waveIndex >= 0 && waveIndex < _waves.Count)
        {
            Wave wave = _waves[waveIndex];
            foreach(var enemy in wave.enemies)
            {
                var spawnWarning = Instantiate(_spawnWarning, enemy.position);
                StartCoroutine(DelaySpawn(enemy, spawnWarning));
            }
        }
    }

    IEnumerator DelaySpawn(EnemySpawnInfo enemy, GameObject spawnWarning)
    {
        yield return new WaitForSeconds(2f);
        var instance = Instantiate(enemy.prefab, enemy.position);
        instance.GetComponent<Enemy>().SetAI(true);
        _aliveEnemies.Add(instance.GetComponent<Enemy>());
        Destroy(spawnWarning);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTimeController : MonoBehaviour
{
    [SerializeField] float _slowMotionFactor = 0.5f;
    [SerializeField] float _normalTimeScale = 1.0f; 
    bool _isBulletTimeActive = false;
    bool _isBulletTimeBeingConsumed = false;

    float _maxBulletTime = 5f;
    float _currentBulletTime = 0f;
    bool _bulletTimeAvalaible = true;

    private void Start()
    {
        _currentBulletTime = _maxBulletTime;
        UIManager.Instance.ChangeCurrentBulletTime(_maxBulletTime);
        UIManager.Instance.ChangeMaxBulletTime(_maxBulletTime);
    }
    void Update()
    {
        if(_isBulletTimeBeingConsumed && _currentBulletTime > 0)
        {
            _currentBulletTime -= Time.deltaTime * 2f;
        }

        if(_currentBulletTime < _maxBulletTime)
        {
            _currentBulletTime += 0.25f * Time.deltaTime;
        }

        if (_currentBulletTime <= 0)
        {
            _bulletTimeAvalaible = false;
            _isBulletTimeActive = true;
            ToggleBulletTime();
        }
        else
        {
            _bulletTimeAvalaible = true;
        }

        UIManager.Instance.ChangeCurrentBulletTime(_currentBulletTime);

        if (Input.GetKeyDown(KeyCode.LeftShift) && _bulletTimeAvalaible)
        {
            Debug.Log("BulletTime");
            ToggleBulletTime();
        }
    }

    void ToggleBulletTime()
    {
        if (_isBulletTimeActive)
        {
            Time.timeScale = _normalTimeScale;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            _isBulletTimeActive = false;
            _isBulletTimeBeingConsumed = false;
        }
        else
        {
            Time.timeScale = _slowMotionFactor;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            _isBulletTimeActive = true;
            _isBulletTimeBeingConsumed = true;
        }
    }

    private void OnApplicationQuit()
    {
        // Ensure time scale resets when quitting
        Time.timeScale = _normalTimeScale;
    }
}

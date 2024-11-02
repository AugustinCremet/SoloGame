using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTimeController : MonoBehaviour
{
    [SerializeField] float slowMotionFactor = 0.5f; // Speed during bullet time
    [SerializeField] float normalTimeScale = 1.0f; // Normal speed
    private bool isBulletTimeActive = false;

    void Update()
    {
        // Toggle bullet time on key press (e.g., left shift)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ToggleBulletTime();
        }
    }

    void ToggleBulletTime()
    {
        if (isBulletTimeActive)
        {
            Time.timeScale = normalTimeScale; // Reset to normal time scale
        }
        else
        {
            Time.timeScale = slowMotionFactor; // Set to slow motion
        }
        isBulletTimeActive = !isBulletTimeActive;
    }

    private void OnApplicationQuit()
    {
        // Ensure time scale resets when quitting
        Time.timeScale = normalTimeScale;
    }
}

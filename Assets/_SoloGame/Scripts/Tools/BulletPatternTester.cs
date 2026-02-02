using UnityEngine;
using BulletPro;
using System.Collections.Generic;

public class BulletPatternTester : MonoBehaviour
{
    private List<BulletEmitter> _emitters = new List<BulletEmitter>();
    [SerializeField] private List<EmitterProfile> _profilesToTest;
    private int _currentProfileIndex = 0;
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            _emitters.Add(transform.GetChild(i).GetComponent<BulletEmitter>());
        }
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("Space key pressed");
            foreach (BulletEmitter emitter in _emitters)
            {
                if(_currentProfileIndex >= _profilesToTest.Count)
                {
                    _currentProfileIndex = -1;
                }
                emitter.emitterProfile = _profilesToTest[_currentProfileIndex + 1];
            }
            _currentProfileIndex++;
        }
    }
}

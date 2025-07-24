using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class Audio_Manager_Music : MonoBehaviour
{
  public static Audio_Manager_Music AudioManagerMusicInstance;

    [Header("Wwise Banks")]
    public AK.Wwise.Bank musicBank;
    [Space]
    [Header("Wwise Events")]
    public AK.Wwise.Event playMenuMusicEvent;
    
   void Awake()
    {
        musicBank.Load();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playMenuMusicEvent.Post(gameObject);
    }

}

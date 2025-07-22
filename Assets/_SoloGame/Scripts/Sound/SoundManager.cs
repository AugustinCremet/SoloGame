using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }

    public void PlaySound(SOSoundPack soundPack, ESoundType soundType, GameObject source)
    {
        soundPack?.Play(soundType, source);
    }


    // For stopping sound
    //public uint PlaySound(SOSoundPack soundPack, ESoundType soundType, GameObject source)
    //{
    //    uint ID = 0;

    //    if(soundPack != null)
    //    {
    //        ID = soundPack.Play(soundType, source);
    //    }

    //    return ID;
    //}

    //public void StopSound(uint ID)
    //{
    //    AkUnitySoundEngine.StopPlayingID(ID);
    //}
}

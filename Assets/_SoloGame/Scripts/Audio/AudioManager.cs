using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] AK.Wwise.Bank _bank;

    private void Awake()
    {
        // AC_TODO temp for 1 bank
        _bank.Load();

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

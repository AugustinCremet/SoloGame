using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] SOSoundPack _soundPack;

    public void Play(ESoundType soundType)
    {
        AudioManager.Instance.PlaySound(_soundPack, soundType, gameObject);
    }

    // For stopping sound
    //public uint Play(ESoundType soundType)
    //{
    //    uint ID = 0;
    //    if (_soundPack != null)
    //    {
    //        ID = SoundManager.Instance.PlaySound(_soundPack, soundType, gameObject);

    //    }
    //}
}

using System;
using UnityEngine;

[Serializable]
public class SoundEntry
{
    public ESoundType SoundType;
    public AK.Wwise.Event WwiseEvent;

    public void Play(GameObject source)
    {
        WwiseEvent?.Post(source);
    }

    // For stopping sound
    //public uint Play(GameObject source)
    //{
    //    uint ID = 0;
    //    if (WwiseEvent != null)
    //    {
    //        ID = WwiseEvent.Post(source);
    //    }

    //    return ID;
    //}
}

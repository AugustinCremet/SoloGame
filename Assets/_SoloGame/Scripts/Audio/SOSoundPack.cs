using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundPack", menuName = "Audio/SoundPack")]
public class SOSoundPack : ScriptableObject
{
    [SerializeField] List<SoundEntry> _sounds = new List<SoundEntry>();
    private Dictionary<ESoundType, SoundEntry> _lookup;

    private void OnEnable()
    {
        _lookup = new Dictionary<ESoundType, SoundEntry>();

        foreach(var entry in _sounds)
        {
            if(!_lookup.ContainsKey(entry.SoundType) && entry.WwiseEvent != null)
            {
                _lookup.Add(entry.SoundType, entry);
            }
        }
    }

    public void Play(ESoundType soundType, GameObject source)
    {
        if (_lookup == null)
        {
            OnEnable();
        }

        if (_lookup.TryGetValue(soundType, out SoundEntry entry))
        {
            entry.Play(source);
        }
    }

    // For stopping sound
    //public uint Play(ESoundType soundType, GameObject source)
    //{
    //    uint ID = 0;
    //    if (_lookup == null)
    //    {
    //        OnEnable();
    //    }

    //    if (_lookup.TryGetValue(soundType, out SoundEntry entry))
    //    {
    //        ID = entry.Play(source);
    //    }

    //    return ID;
    //}
}

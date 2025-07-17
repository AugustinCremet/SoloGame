using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Blackboard
{
    private Dictionary<string, object> _data = new Dictionary<string, object>();

    public void Set<T>(string key, T value)
    {
        _data[key] = value;
    }

    public T Get<T>(string key)
    {
        return _data.TryGetValue(key, out var value) ? (T)value : default;
    }

    public bool TryGet<T>(string key, out T value)
    {
        if(_data.TryGetValue(key, out var val) && val is T t)
        {
            value = t;
            return true;
        }

        value = default;
        return false;
    }
}

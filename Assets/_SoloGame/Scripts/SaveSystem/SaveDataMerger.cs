using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public static class SaveDataMerger
{
    public static void Merge<T>(T target, T source)
    {
        if (target == null || source == null) return;

        Type type = typeof(T);

        // Handle properties
        foreach (PropertyInfo property in type.GetProperties())
        {
            MergeFieldOrProperty(target, source, property, property.GetValue, property.SetValue);
        }

        // Handle fields (important for PlayerData staying null)
        foreach (FieldInfo field in type.GetFields())
        {
            MergeFieldOrProperty(target, source, field, field.GetValue, field.SetValue);
        }
    }

    private static void MergeFieldOrProperty<T>(
        T target, T source, MemberInfo member,
        Func<object, object> getValue, Action<object, object> setValue)
    {
        object sourceValue = getValue(source);
        if (sourceValue != null)
        {
            if (sourceValue is IList list)
            {
                IList targetList = (IList)getValue(target);
                if (targetList == null)
                {
                    targetList = (IList)Activator.CreateInstance(member switch
                    {
                        PropertyInfo p => p.PropertyType,
                        FieldInfo f => f.FieldType,
                        _ => throw new InvalidOperationException()
                    });
                    setValue(target, targetList);
                }

                foreach (var item in list)
                {
                    targetList.Add(item);
                }
            }
            else
            {
                setValue(target, sourceValue);
            }
        }
    }
}

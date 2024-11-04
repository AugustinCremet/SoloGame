using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewElement", menuName = "Elements/Element")]
public class Element : ScriptableObject
{
    public ElementType elementType;
    public List<ElementType> weaknesses;
    public List<ElementType> resistances;
    public List<ElementType> immunities;

    public float CalculateDamageFrom(Element attacker, float baseDamage)
    {
        float multiplier = 1f;

        if (attacker.weaknesses.Contains(elementType))
        {
            multiplier *= 0.5f;
        }
        else if (attacker.resistances.Contains(elementType))
        {
            multiplier *= 2f;
        }
        else if(attacker.immunities.Contains(elementType))
        {
            multiplier = 0f;
        }

        return baseDamage * multiplier;
    }
}

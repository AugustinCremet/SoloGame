using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewElement", menuName = "Elements/Element")]
public class Element : ScriptableObject
{
    public ElementType ElementType;
    public List<ElementType> Weaknesses;
    public List<ElementType> Resistances;
    public List<ElementType> Immunities;

    public float CalculateDamageFrom(Element attacker, float baseDamage)
    {
        float multiplier = 1f;

        if (attacker.Weaknesses.Contains(ElementType))
        {
            multiplier *= 0.5f;
        }
        else if (attacker.Resistances.Contains(ElementType))
        {
            multiplier *= 2f;
        }
        else if(attacker.Immunities.Contains(ElementType))
        {
            multiplier = 0f;
        }

        return baseDamage * multiplier;
    }
}

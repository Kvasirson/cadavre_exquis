using UnityEngine;

[CreateAssetMenu(fileName = "NewPart", menuName = "ScriptableObjects/CreaturePart")]
public class PartObject : ScriptableObject
{
    [SerializeField]
    Sprite m_partSprite;

    [SerializeField]
    PartTypes m_partType;

    [Header("Attributes")]
    [SerializeField]
    [Range(0f, 1f)]
    float m_cutenessValue;

    [SerializeField]
    [Range(0f, 1f)]
    float m_tastinessValue;

    [SerializeField]
    [Range(0f, 1f)]
    float m_strengthValue;

    [SerializeField]
    [Range(0f, 1f)]
    float m_exotismValue;

    float[] _attributeValues;

    float[] AttributeValues
    {
        get 
        { 
            if (_attributeValues == null) 
            {
                float[] values = new float[4];
                values[0] = m_cutenessValue;
                values[1] = m_tastinessValue;
                values[2] = m_strengthValue;
                values[3] = m_exotismValue;

                _attributeValues = values;
            }

            return _attributeValues;
        }
    }

    public Sprite PartSprite
    {
        get { return m_partSprite; }
    }

    public PartTypes ObjectType
    {
        get { return m_partType; }
    }

    public float GetAttributeValue(PartsAttributes attribute)
    {
        float value = 0f;

        switch (attribute)
        {
            case PartsAttributes.Cuteness: 
                value = m_cutenessValue;
                break;
            case PartsAttributes.Tastiness:
                value = m_tastinessValue;
                break;
            case PartsAttributes.Strength:
                value = m_strengthValue;
                break;
            case PartsAttributes.Exotism:
                value = m_exotismValue;
                break;
        }

        return value;
    }

    public PartsAttributes GetDominantAttribute()
    {
        int index = 0;
        float dominantValue = 0;

        for (int i = 0; i < AttributeValues.Length; i++)
        {
            float value = AttributeValues[i];

            if (value > dominantValue)
            {
                dominantValue = value;
                index = i;
            }
        }
        
        return (PartsAttributes)index;
    }
}

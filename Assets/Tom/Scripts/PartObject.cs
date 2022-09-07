using UnityEngine;

[CreateAssetMenu(fileName = "NewPart", menuName = "ScriptableObjects/CreaturePart")]
public class PartObject : ScriptableObject
{
    [SerializeField]
    Sprite m_partSprite;

    [SerializeField]
    string m_partName;

    [SerializeField]
    PartTypes m_partType;

    [Header("Attributes")]
    [SerializeField]
    [Range(0f, 1f)]
    float m_attribute1Value;

    [SerializeField]
    [Range(0f, 1f)]
    float m_attribute2Value;

    [SerializeField]
    [Range(0f, 1f)]
    float m_attribute3Value;

    [SerializeField]
    [Range(0f, 1f)]
    float m_attribute4Value;

    public PartTypes GetObjectType
    {
        get { return m_partType; }
    }

    public int GetDominantAttributeIndex()
    {
        int index = 0;
        float dominantValue = 0;

        float[] values = new float[4];
        values[0] = m_attribute1Value;
        values[1] = m_attribute2Value;
        values[2] = m_attribute3Value;
        values[3] = m_attribute4Value;

        for (int i = 0; i < values.Length; i++)
        {
            float value = values[i];

            if (value > dominantValue)
            {
                dominantValue = value;
                index = i;
            }
        }

        return index;
    }
}

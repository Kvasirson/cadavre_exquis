using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShopScript : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    float m_favoriteAttributeChance;

    PartObject[] _objectsPool;
    List<PartObject> _tempObjectsPool;

    List<PartTypes> _usedTypes;

    PartObject [] CreatePool()
    {
        string[] partObjectsGUIDs = AssetDatabase.FindAssets("t:PartObject", new[] { "Assets/ScriptableObjects" });

        PartObject[] pool = new PartObject[partObjectsGUIDs.Length];

        for (int i = 0; i < pool.Length; i++)
        {
            string partObjectGUID = partObjectsGUIDs[i];
            string path = AssetDatabase.GUIDToAssetPath(partObjectGUID);
            PartObject partObject = AssetDatabase.LoadAssetAtPath<PartObject>(path);
            pool[i] = partObject;
        }

        return pool;
    }

    void GetAvailableObjects()
    {
        List<PartObject> availableObjects = new List<PartObject>();
        foreach (PartObject obj in _tempObjectsPool)
        {
            bool isExcluded = false;
            foreach (PartTypes excludedType in _usedTypes)
            {
                if (obj.GetObjectType == excludedType)
                {
                    isExcluded = true;
                    break;
                }
            }

            if (!isExcluded)
            {
                availableObjects.Add(obj);
            }
        }

        _tempObjectsPool = availableObjects;
    }

    PartObject GetObject(int favoriteAttributeIndex)
    {
        List<PartObject> favoriteObjects = new List<PartObject>();
        List<PartObject> otherObjects = _tempObjectsPool;

        foreach(PartObject obj in _tempObjectsPool)
        {
            if(obj.GetDominantAttributeIndex() == favoriteAttributeIndex)
            {
                favoriteObjects.Add(obj);
                otherObjects.Remove(obj);
            }
        }

        float chance = Random.Range(0f, 1f);
        if (chance < m_favoriteAttributeChance)
        {
            int randomIndex = Random.Range(0, favoriteObjects.Count);

            return favoriteObjects[randomIndex];
        }
        else
        {
            int randomIndex = Random.Range(0, otherObjects.Count);

            return otherObjects[randomIndex];
        }

    }

    void ResetTempPool()
    {
        _tempObjectsPool = new List<PartObject>(_objectsPool);
    }
}

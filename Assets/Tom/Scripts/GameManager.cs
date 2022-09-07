using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    [Header("Shops")]
    [SerializeField]
    [Range(0f, 1f)]
    float m_favoriteAttributeChance;

    [SerializeField]
    List<ShopScript> m_shopsScripts;

    static GameManager _instance;

    List<PartTypes> _usedTypes;

    PartObject[] _objectsPool;
    List<PartObject> _tempObjectsPool;

    public static GameManager GetInstance
    {
        get { return _instance; }
    }

    public List<PartTypes> UsedTypes
    {
        get { return _usedTypes; }
    }

    private void Awake()
    {
        _instance = this;
        _objectsPool = CreatePool();
    }

    #region Shops
    PartObject[] CreatePool()
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

    public void UpdateShops()
    {
        GetAvailableObjects();

        foreach (ShopScript shop in m_shopsScripts)
        {
            shop.DisplayedObject = GetObject(shop.FavoriteAttribute);
        }
    }

    void GetAvailableObjects()
    {
        List<PartObject> availableObjects = new List<PartObject>();
        foreach (PartObject obj in _tempObjectsPool)
        {
            bool isExcluded = false;
            foreach (PartTypes excludedType in UsedTypes)
            {
                if (obj.ObjectType == excludedType)
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

    PartObject GetObject(PartsAttributes favoriteAttribute)
    {
        List<PartObject> favoriteObjects = new List<PartObject>();
        List<PartObject> otherObjects = _tempObjectsPool;

        foreach (PartObject obj in _tempObjectsPool)
        {
            if (obj.GetDominantAttribute() == favoriteAttribute)
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

    public void RemoveObjet(PartObject obj)
    {
        _tempObjectsPool.Remove(obj);
    }

    void ResetCurPool()
    {
        _tempObjectsPool = new List<PartObject>(_objectsPool);
    }
    #endregion
}

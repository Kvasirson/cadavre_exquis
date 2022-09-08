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
    int m_basePartSoldValue;

    [SerializeField]
    float m_eggTimerDuration;

    [SerializeField]
    float m_eggTimerBonusDuration;

    [SerializeField]
    List<ShopScript> m_shopsScripts;

    [SerializeField]
    EggScript m_eggScript;

    static GameManager _instance;

    public float _gold;

    [HideInInspector]
    public List<PartTypes> _usedTypes;

    [HideInInspector]
    public bool _eggIscomplete;

    float timerCurTime;
    Coroutine curTimer;

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

    public int BasePartSoldValue
    {
        get { return m_basePartSoldValue; }
    }

    private void Awake()
    {
        _instance = this;
        _objectsPool = CreatePool();
        ResetCurPool();
    }

    private void Start()
    {
        UpdateShops();
    }

    #region Shops
    PartObject[] CreatePool()
    {
        string[] partObjectsGUIDs = AssetDatabase.FindAssets("t:PartObject", new[] { "Assets/Tom/ScriptableObjects" });

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
        curTimer = StartCoroutine(Timer());

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

            if (UsedTypes != null)
            {
                foreach (PartTypes excludedType in UsedTypes)
                {
                    if (obj.ObjectType == excludedType)
                    {
                        isExcluded = true;
                        break;
                    }
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
        PartObject[] tempOtherObjects = new PartObject[_tempObjectsPool.Count];
        _tempObjectsPool.CopyTo(tempOtherObjects);
        List<PartObject> otherObjects = new List<PartObject>(tempOtherObjects);

        foreach (PartObject obj in _tempObjectsPool)
        {
            if (obj.GetDominantAttribute() == favoriteAttribute)
            {
                favoriteObjects.Add(obj);
                otherObjects.Remove(obj);
            }
        }

        PartObject selectedObject;

        float chance = Random.Range(0f, 1f);
        if (chance < m_favoriteAttributeChance)
        {
            if (favoriteObjects.Count <= 0)
            {
                return null;
            }

            int randomIndex = Random.Range(0, favoriteObjects.Count);

            selectedObject = favoriteObjects[randomIndex];
        }
        else
        {
            if (otherObjects.Count <= 0)
            {
                return null;
            }

            int randomIndex = Random.Range(0, otherObjects.Count);

            selectedObject = otherObjects[randomIndex];
        }

        return selectedObject;
    }

    public void RemoveObjet(PartObject obj)
    {
        _tempObjectsPool.Remove(obj);
    }

    public void ResetCurPool()
    {
        _tempObjectsPool = new List<PartObject>(_objectsPool);
    }
    #endregion

    IEnumerator Timer ()
    {
        timerCurTime = 0f;
        timerCurTime += m_eggTimerDuration;

        while (timerCurTime > 0f)
        {
            timerCurTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        TimerEnd();
    }

    void TimerEnd()
    {
        Debug.Log("Egg reset");
        //m_eggScript.Reset();
    }

    public void StopTimer()
    {
        StopCoroutine(curTimer);
        timerCurTime = 0f;
    }

    public void IncreaseTimer()
    {
        timerCurTime += m_eggTimerBonusDuration;
    }
}

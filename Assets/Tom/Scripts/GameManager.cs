using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    [Header("Shops")]
    [Range(0f, 1f)]
    [SerializeField]
    float m_favoriteAttributeChance;

    [Header("PartsSoldValue")]
    public int m_base1PartSoldValue;
    public int m_base2PartsSoldValue;
    public int m_base3PartsSoldValue;
    public int m_base4PartsSoldValue;
    public int m_base5PartsSoldValue;

    [Header("Base Gold")]
    [SerializeField]
    float _startingGold;

    [SerializeField]
    float m_globalTimerDuration;

    [SerializeField]
    float m_eggTimerDuration;

    [SerializeField]
    float m_eggTimerBonusDuration;

    [Header("UI")]
    [SerializeField]
    GameObject m_endScreen;

    [SerializeField]
    Text m_globalTimerDisplay;

    [SerializeField]
    Text m_eggTimerDisplay;

    [SerializeField]
    Text m_goldCountDisplay;

    [Header("Scripts")]
    [SerializeField]
    List<ShopScript> m_shopsScripts;

    [SerializeField]
    EggScript m_eggScript;

    static GameManager _instance;

    [HideInInspector]
    public List<PartTypes> _usedTypes;

    [HideInInspector]
    public bool _eggIscomplete;

    float eggTimerCurTime;
    Coroutine curEggTimer;

    [SerializeField]
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

    public float Gold
    {
        get { return _startingGold; }
        set 
        { 
            _startingGold = value; 
            m_goldCountDisplay.text = "Gold : " + value.ToString();
        }
    }

    public int GetSoldValue(int partsNb)
    {
        switch (partsNb)
        {
            case 1:
                return m_base1PartSoldValue;
            case 2:
                return m_base2PartsSoldValue;
            case 3:
                return m_base3PartsSoldValue;
            case 4: 
                return m_base4PartsSoldValue;
            case 5:
                return m_base5PartsSoldValue;
            default:
                return 0;
        }
    }

    private void Awake()
    {
        _instance = this;
        m_endScreen.SetActive(false);
        //_objectsPool = CreatePool();
        ResetCurPool();
    }

    private void Start()
    {
        StartCoroutine(GlobalTimer());
        Gold = _startingGold;
        UpdateShops();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            SceneManager.LoadScene(1);
        }
    }

    #region Shops
   /* PartObject[] CreatePool()
    {
#if UNITY_EDITOR

        string[] partObjectsGUIDs = AssetDatabase.FindAssets("t:PartObject", new[] { "Assets/Tom/ScriptableObjects" });

        PartObject[] pool = new PartObject[partObjectsGUIDs.Length];

        for (int i = 0; i < pool.Length; i++)
        {
            string partObjectGUID = partObjectsGUIDs[i];
            string path = AssetDatabase.GUIDToAssetPath(partObjectGUID);
            PartObject partObject = AssetDatabase.LoadAssetAtPath<PartObject>(path);
            pool[i] = partObject;
        }
#endif

        return pool;
    }*/

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

    #region Timer
    IEnumerator EggTimer ()
    {
        eggTimerCurTime = 0f;
        eggTimerCurTime += m_eggTimerDuration;

        while (eggTimerCurTime > 0f)
        {
            eggTimerCurTime -= Time.deltaTime;
            m_eggTimerDisplay.text = "Egg Timer : " + eggTimerCurTime.ToString().Substring(0, 2);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        EggTimerEnd();
    }

    void EggTimerEnd()
    {
        if (_startingGold <= 0)
        {
            GameEnd();
            return;
        }

        Debug.Log("Egg reset");
        m_eggScript.Reset();
        ResetCurPool();
        curEggTimer = null;
    }

    public void StopEggTimer()
    {
        StopCoroutine(curEggTimer);
        eggTimerCurTime = 0f;
        curEggTimer = null;
    }

    public void IncreaseEggTimer()
    {
        if(curEggTimer == null)
        {
            curEggTimer = StartCoroutine(EggTimer());
        }
        else
        {
            eggTimerCurTime += m_eggTimerBonusDuration;
        }
    }

    IEnumerator GlobalTimer()
    {
        float curTime = m_globalTimerDuration;

        while (curTime > 0f)
        {
            curTime -= Time.deltaTime;
            m_globalTimerDisplay.text = "Global Timer : " + curTime.ToString().Substring(0, 2);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        GameEnd();
    }
    #endregion

    public void GameEnd()
    {
        m_endScreen.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }
}

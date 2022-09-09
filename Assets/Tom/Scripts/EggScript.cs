using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour
{
    [SerializeField]
    private GameObject m_eggDisplay;

    [SerializeField]
    private GameObject m_creatureDisplay;

    PartObject[] _slots = new PartObject[5];

    GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.GetInstance; 
    }

    public void AddPart(PartObject part)
    {
        PartTypes type = part.ObjectType;

        switch (type)
        {
            case PartTypes.Head:
                _slots[0] = part;
                break;
            case PartTypes.Torso:
                _slots[1] = part;
                break;
            case PartTypes.Arms:
                _slots[2] = part;
                break;
            case PartTypes.Legs:
                _slots[3] = part;
                break;
            case PartTypes.Tail:
                _slots[4] = part;
                break;
        }

        if (EggCompleteCheck())
        {
            _gameManager._eggIscomplete = true;
            _gameManager.StopEggTimer();
            _gameManager._usedTypes.Clear();

            DisplayEgg(false);
            m_creatureDisplay.SetActive(true);
        }
        else
        {
            _gameManager._usedTypes.Add(type);
        }

        _gameManager.UpdateShops();
    }

    public float SoldValue(PartsAttributes vendorAttribute)
    {
        float multiplier = 0f;
        int partsNumb = 0;

        foreach (PartObject part in _slots)
        {
            if (part != null)
            {
                multiplier += part.GetAttributeValue(vendorAttribute);
                partsNumb++;
            }
        }

        int soldValue = _gameManager.GetSoldValue(partsNumb);

        multiplier /= partsNumb;
        return multiplier*soldValue;
    }

    public bool IsEmpty
    {
        get
        {
            bool isEmpty = true;

            foreach (PartObject part in _slots)
            {
                if (part != null)
                {
                    isEmpty = false;
                    break;
                }
            }

            return isEmpty;
        }
    }

    bool EggCompleteCheck()
    {
        bool emptySlot = false;
        for (int i = 0; i < _slots.Length; i++)
        {
            if(_slots[i] == null)
            {
                emptySlot = true;
                break;
            }
        }

        return !emptySlot;
    }
    
    public void Reset()
    {
        for(int i = 0; i < _slots.Length; i++)
        {
            _slots[i] = null;
        }

        _gameManager.StopEggTimer();

        DisplayEgg(true);
    }

    public void DisplayEgg(bool display)
    {
        if (m_creatureDisplay)
        {
            m_creatureDisplay.SetActive(false);
        }
        else
        {
            m_eggDisplay.SetActive(display);
        }
    }
}

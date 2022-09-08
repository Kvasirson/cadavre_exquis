using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour
{
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
            _gameManager.StopTimer();
            _gameManager._usedTypes.Clear();
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
        int baseSoldValue = _gameManager.BasePartSoldValue;

        foreach (PartObject part in _slots)
        {
            if (part != null)
            {
                multiplier += part.GetAttributeValue(vendorAttribute);
                partsNumb++;
            }
        }

        multiplier /= partsNumb;
        return multiplier*baseSoldValue;
    }

    bool EggCompleteCheck()
    {
        bool emptySlot = false;
        for (int i = 0; i < _slots.Length; i++)
        {
            Debug.Log(_slots[i]);

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

        _gameManager.StopTimer();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour
{
    PartObject _head;
    PartObject _torso;
    PartObject _arms;
    PartObject _legs;
    PartObject _tail;

    PartObject[] _slots;

    PartObject[] Slots
    {
        get 
        { 
            if(_slots == null)
            {
                _slots = new PartObject[5];
                _slots[0] = _head;
                _slots[1] = _torso;
                _slots[2] = _arms;
                _slots[3] = _legs;
                _slots[4] = _tail;
            }
            return _slots;
        }
    }

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
                _head = part;
                break;
            case PartTypes.Torso:
                _torso = part;
                break;
            case PartTypes.Arms:
                _arms = part;
                break;
            case PartTypes.Legs:
                _legs = part;
                break;
            case PartTypes.Tail:
                _tail = part;
                break;
        }

        _gameManager.UsedTypes.Add(type);
        _gameManager.UpdateShops();
    }

    public float SoldValue(PartsAttributes vendorAttribute)
    {
        float multiplier = 0f;
        int partsNumb = 0;
        int baseSoldValue = _gameManager.BasePartSoldValue;

        foreach (PartObject part in Slots)
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

    public void Reset()
    {
        for(int i = 0; i < Slots.Length; i++)
        {
            Slots[i] = null;
        }

        _gameManager.StopTimer();
    }
}

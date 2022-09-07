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
}

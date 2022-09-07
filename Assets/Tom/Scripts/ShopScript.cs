using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    [SerializeField]
    PartsAttributes m_favoriteAttribute;

    [SerializeField]
    SpriteRenderer m_objectSpriteRend;

    PartObject _displayedObject;

    int _objectPrice;

    public PartsAttributes FavoriteAttribute
    {
        get { return m_favoriteAttribute; }
    }

    public PartObject DisplayedObject
    {
        get { return _displayedObject; }

        set 
        { 
            _displayedObject = value;
            ChangeDisplayedObject();
        }
    }

    public int ObjectPrice
    {
        get { return _objectPrice; }
    }

    void ChangeDisplayedObject()
    {
        if(_displayedObject != null)
        {
            m_objectSpriteRend.sprite = _displayedObject.partSprite;
            _objectPrice = Random.Range(0, 100);
        }
        else
        {
            m_objectSpriteRend = null;
        }
    }
}

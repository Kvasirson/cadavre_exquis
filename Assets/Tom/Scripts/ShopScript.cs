using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    [SerializeField]
    PartsAttributes m_favoriteAttribute;

    [SerializeField]
    GameObject m_objectDisplay;

    [SerializeField]
    SpriteRenderer m_objectSpriteRend;
    [SerializeField]
    Text m_objectPrice;

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
            m_objectSpriteRend.sprite = _displayedObject.PartSprite;
            _objectPrice = Random.Range(0, 100);
            m_objectPrice.text = _objectPrice.ToString();
        }
        else
        {
            m_objectSpriteRend = null;
        }
    }

    public void ShowDisplayedObject(bool showDisplay)
    {
        m_objectDisplay.SetActive(showDisplay);
    }
}

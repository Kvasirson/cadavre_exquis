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
    Text m_objectFlavorText;

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
            m_objectFlavorText.text = _displayedObject.FlavorText;
            _objectPrice = Random.Range(0, 100);
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

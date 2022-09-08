using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayering : MonoBehaviour
{
    [SerializeField]
    GameObject m_player;

    SpriteRenderer m_spriteRenderer;
    SpriteRenderer m_playerRenderer;

    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_playerRenderer = m_player.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float playerYPosition = m_player.transform.position.y - m_playerRenderer.sprite.bounds.extents.y;
        float spriteYPosition = transform.position.y - m_spriteRenderer.sprite.bounds.extents.y;

        if(playerYPosition > spriteYPosition)
        {
            m_spriteRenderer.sortingLayerID = m_playerRenderer.sortingLayerID - 1;
        }
        else
        {
            m_spriteRenderer.sortingLayerID = m_playerRenderer.sortingLayerID + 1;
        }
    }
}

using UnityEngine;
using UnityEngine.Rendering;

public class SpriteLayering : MonoBehaviour
{
    [SerializeField]
    GameObject m_player;

    [SerializeField]
    Sprite m_playerSprite;

    SpriteRenderer m_spriteRenderer;
    SortingGroup m_playerRenderer;

    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_playerRenderer = m_player.GetComponent<SortingGroup>();
    }

    void Update()
    {
        float playerYPosition = m_player.transform.position.y - m_playerSprite.bounds.extents.y;
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

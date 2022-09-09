using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureScript : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer HeadRenderer;
    [SerializeField]
    SpriteRenderer TorsoRenderer;
    [SerializeField]
    SpriteRenderer ArmsRenderer;
    [SerializeField]
    SpriteRenderer LegsRenderer;
    [SerializeField]
    SpriteRenderer TailRenderer;

    public void Create(Sprite head, Sprite torso, Sprite arms, Sprite legs, Sprite tail)
    {
        HeadRenderer.sprite = head;
        TorsoRenderer.sprite = torso;
        ArmsRenderer.sprite = arms;
        LegsRenderer.sprite = legs;
        TailRenderer.sprite = tail;
    }
}

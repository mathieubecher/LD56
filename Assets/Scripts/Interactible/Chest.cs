using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    [SerializeField] private SpriteRenderer m_chestSprite;
    [SerializeField] private Sprite m_openSprite;
    [SerializeField] private string m_contains;
    [SerializeField] private Cinematic m_openCinematic;
    
    protected override void PlayEffect()
    {
        GameManager.AddItem(m_contains, 1);
        CinematicManager.instance.Play(m_openCinematic);
    }

    public override void Activate()
    {
        m_chestSprite.sprite = m_openSprite;
    }
}

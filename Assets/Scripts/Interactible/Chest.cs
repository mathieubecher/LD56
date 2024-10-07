using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactible
{
    [SerializeField] private SpriteRenderer m_chestSprite;
    [SerializeField] private Sprite m_openSprite;
    [SerializeField] private string m_contains;
    [SerializeField] private Cinematic m_openCinematic;

    private bool m_open;

    private void OnEnable()
    {
        if (GameManager.HasItem(m_contains))
        {
            m_open = true;
            m_chestSprite.sprite = m_openSprite;
        }
    }
    
    public override void Interact()
    {
        if (m_open) return;
        
        m_open = true;
        m_chestSprite.sprite = m_openSprite;
        GameManager.AddItem(m_contains, 1);
        CinematicManager.instance.Play(m_openCinematic);
    }
}

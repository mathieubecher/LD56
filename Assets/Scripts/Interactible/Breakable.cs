using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : Hitable
{
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Sprite m_breakSprite;
    [SerializeField] private Collider2D m_collider;
    private bool m_break;
    
    protected override void Hit(Vector2 _source, int _damage)
    {
        if (m_break) return;
        
        m_break = true;
        m_spriteRenderer.sprite = m_breakSprite;
        if (TryGetComponent(out Loot _loot))
        {
            _loot.CreateLoot();
        }

        m_collider.enabled = false;
    }
}

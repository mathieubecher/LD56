using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingHitable : Hitable
{
    [Header("Living")]
    [SerializeField] private Life m_life;
    
    protected override void Hit(Vector2 _source, int _damage)
    {
        if (m_life.Hit(_damage))
        {
            if(m_life.dead) OnDead();
            else OnDamaged(_source, _damage);
        }
    }

    protected virtual void OnDamaged(Vector2 _source, int _damage)
    {
        Debug.Log("Hit");
    }

    protected virtual void OnDead()
    {
        Debug.Log("Dead");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private float m_angle = 360.0f;
    [SerializeField] private int m_damage = 1;
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (GameManager.HitRelation(this.tag, _other.tag))
        {
            Vector2 dir = transform.right;
            dir.x *= transform.lossyScale.x;
            dir.y *= transform.lossyScale.x;
            
            float angle = Vector2.Angle(dir, _other.transform.position - transform.position);
            
            if (angle <= m_angle)
            {
                if(_other.TryGetComponent(out Hurtbox _hurtbox))
                {
                    _hurtbox.Hit(transform.position, m_damage);
                }
            }
        }
    }
}

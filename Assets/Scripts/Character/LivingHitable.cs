using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingHitable : Hitable
{
    [Header("Living")]
    [SerializeField] protected Rigidbody2D m_rigidbody;
    [SerializeField] protected Life m_life;
    [SerializeField] protected float m_invulnerability = 0.5f;
    [SerializeField] protected float m_hitPush = 0.3f;
    [SerializeField] protected float m_hitPushStrength = 5.0f;
    [SerializeField] protected float m_hitPushFriction = 0.9f;

    private float m_invulnerabilityTimer;
    protected float m_hitPushTimer;

    protected void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    protected void Update()
    {
        m_invulnerabilityTimer -= Time.deltaTime;
        m_hitPushTimer -= Time.deltaTime;
    }
    
    protected void FixedUpdate()
    {
        if (m_hitPushTimer > 0.0f)
        {
            m_rigidbody.velocity *= m_hitPushFriction;
        }
        else if(m_life.dead) m_rigidbody.velocity = Vector2.zero;
    }
    protected override void Hit(Vector2 _source, int _damage)
    {
        if (m_invulnerabilityTimer > 0.0f) return;
        if (m_life.Hit(_damage))
        {
            m_invulnerabilityTimer = m_invulnerability;
            m_rigidbody.velocity = ((Vector2)transform.position - _source).normalized * m_hitPushStrength;
            m_hitPushTimer = m_hitPush;
            if(m_life.dead) OnDead();
            else OnDamaged(_source, _damage);
        }
    }

    protected virtual void OnDamaged(Vector2 _source, int _damage)
    {
    }

    protected virtual void OnDead()
    {
    }
    
}

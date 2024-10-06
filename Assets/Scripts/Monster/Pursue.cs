using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Pursue : LivingHitable
{
    [Header("Pursue")]
    [SerializeField] private Animator m_animator;
    [SerializeField] private float m_maxPursueDistance;
    [SerializeField] private float m_maxDetectionDistance;
    [SerializeField] private float m_pursueSpeed;
    [SerializeField] private float m_pursueInertia;
    
    
    
    [SerializeField] private bool m_detectPlayer;

    private float playerDistance => Vector2.Distance(transform.position, GameManager.character.transform.position);
    private Vector2 playerDirection => ((Vector2)GameManager.character.transform.position - (Vector2)transform.position).normalized;
    
    private void Update()
    {
        base.Update();

        if (playerDistance <= m_maxDetectionDistance)
        {
            m_detectPlayer = true;
        }
        else if(playerDistance > m_maxPursueDistance)
        {
            m_detectPlayer = false;
        } 
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
        if (m_hitPushTimer <= 0.0f)
        {
            if(m_detectPlayer) 
                m_rigidbody.velocity = Vector2.Lerp(m_rigidbody.velocity, playerDirection * m_pursueSpeed, m_pursueInertia);
            else 
                m_rigidbody.velocity = Vector2.zero;
            
            UpdateDirection();
        } 
    }

    protected override void OnDamaged(Vector2 _source, int _damage)
    {
        base.OnDamaged(_source, _damage);
        m_animator.SetBool("move", false);
    }

    public void UpdateDirection()
    {
        if (!GameManager.instance.hasControl) return;
        float minSpeed = 0.1f;
        float minOrientation = 0.7f;
        Vector2 moveDir = m_rigidbody.velocity.normalized;
        m_animator.SetBool("move", m_rigidbody.velocity.magnitude > minSpeed && m_hitPushTimer <= 0.0f);
        
        m_animator.SetBool("side", math.abs(moveDir.x) > minOrientation && (m_animator.GetBool("side") || math.abs(moveDir.y) < minOrientation));
        m_animator.SetBool("north", moveDir.y >= minOrientation && (m_animator.GetBool("north") || math.abs(moveDir.x) < minOrientation));
        m_animator.SetBool("south", moveDir.y <= -minOrientation && (m_animator.GetBool("south") || math.abs(moveDir.x) < minOrientation));

        if(m_animator.GetBool("side")) transform.localScale = new Vector3(math.sign(moveDir.x), 1.0f, 1.0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Pursue : Monster
{
    [Header("Pursue")]
    [SerializeField] private float m_maxPursueDistance;
    [SerializeField] private float m_maxDetectionDistance;
    [SerializeField] private float m_pursueSpeed;
    [SerializeField] private float m_pursueInertia;
    
    private bool m_detectPlayer;

    private float playerDistance => Vector2.Distance(transform.position, GameManager.character.transform.position);
    private Vector2 playerDirection => ((Vector2)GameManager.character.transform.position - (Vector2)transform.position).normalized;
    
    protected override void Update()
    {
        base.Update();

        if (GameManager.character.life.dead)
        {
            m_detectPlayer = false;
            return;
        }
        
        if (playerDistance <= m_maxDetectionDistance)
        {
            m_detectPlayer = true;
        }
        else if(playerDistance > m_maxPursueDistance)
        {
            m_detectPlayer = false;
        } 
    }

    protected override void Move()
    {
        if(m_detectPlayer) 
            m_rigidbody.velocity = Vector2.Lerp(m_rigidbody.velocity, playerDirection * m_pursueSpeed, m_pursueInertia);
        else 
            m_rigidbody.velocity = Vector2.zero;

    }

}

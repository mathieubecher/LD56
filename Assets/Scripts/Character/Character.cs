using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Character : LivingHitable
{
    private bool m_hasControl = true;
    private Rigidbody2D m_rigidbody;
    private Animator m_locomotion;
    
    [Header("Character")]
    [SerializeField] private Animator m_animator;
    [SerializeField] private float m_attackBuffer = 0.2f;
    [SerializeField] private float m_dodgeBuffer = 0.2f;
    
    private float m_currentAttackBuffer;
    private float m_currentDodgeBuffer;

    public Animator animator => m_animator;
    public int currentLife => m_life.currentLife;
    public Vector2 velocity
    {
        get => m_rigidbody.velocity;
        set => m_rigidbody.velocity = value;
    } 
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_locomotion = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        base.OnEnable();
        Controller.OnAttackPress += Attack;
        Controller.OnDodgePress += Dodge;
    }

    private void OnDisable()
    {
        base.OnDisable();
        Controller.OnAttackPress -= Attack;
        Controller.OnDodgePress -= Dodge;
    }

    private void Update()
    {
        BufferManagment();
    }
    public void UpdateDirection()
    {
        Vector2 moveDir = Controller.lastValidDir;
        m_animator.SetBool("side", math.abs(moveDir.x) > 0.1f  && (m_animator.GetBool("side") || math.abs(moveDir.y) < 0.1f));
        m_animator.SetBool("north", moveDir.y >= 0.1f && (m_animator.GetBool("north") || math.abs(moveDir.x) < 0.1f));
        m_animator.SetBool("south", moveDir.y <= -0.1f && (m_animator.GetBool("south") || math.abs(moveDir.x) < 0.1f));

        if(m_animator.GetBool("side")) transform.localScale = new Vector3(math.sign(moveDir.x), 1.0f, 1.0f);
    }

    
    private void BufferManagment()
    {
        if (m_currentAttackBuffer > 0.0f)
        {
            m_currentAttackBuffer -= Time.deltaTime;
            if (m_currentAttackBuffer <= 0.0f)
                m_locomotion.ResetTrigger("Attack");
        }
        if (m_currentDodgeBuffer > 0.0f)
        {
            m_currentDodgeBuffer -= Time.deltaTime;
            if (m_currentDodgeBuffer <= 0.0f)
                m_locomotion.ResetTrigger("Dodge");
        }
    }
    private void Attack()
    {
        if (m_hasControl)
        {
            m_locomotion.SetTrigger("Attack");
            m_currentAttackBuffer = m_attackBuffer;
        }
    }
    
    private void Dodge()
    {
        if (m_hasControl)
        {
            m_locomotion.SetTrigger("Dodge");
            m_currentDodgeBuffer = m_dodgeBuffer;
        }
    }

    protected override void OnDamaged(Vector2 _source, int _damage)
    {
        if (m_hasControl)
        {
            m_locomotion.SetTrigger("Hit");
        }
    }

    protected override void OnDead()
    {
        m_locomotion.SetBool("dead", true);
    }
    
}

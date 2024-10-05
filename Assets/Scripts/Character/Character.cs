using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private bool m_hasControl = true;
    private Rigidbody2D m_rigidbody;
    private Animator m_locomotion;
    
    [SerializeField] private Animator m_animator;
    [SerializeField] private float m_attackBuffer = 0.2f;
    [SerializeField] private float m_dodgeBuffer = 0.2f;

    public Animator animator => m_animator;
    public Vector2 velocity
    {
        get => m_rigidbody.velocity;
        set => m_rigidbody.velocity = value;
    } 
    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_locomotion = GetComponent<Animator>();
    }

    void OnEnable()
    {
        Controller.OnAttackPress += Attack;
        Controller.OnDodgePress += Dodge;
    }

    private void OnDisable()
    {
        Controller.OnAttackPress -= Attack;
        Controller.OnDodgePress -= Dodge;
    }

    void Update()
    {
        if(Controller.tilt > 0.0f)
        {
            Vector2 moveDir = Controller.moveDir;
            m_animator.SetBool("west", moveDir.x <= -0.1f);
            m_animator.SetBool("east", moveDir.x >= 0.1f);
            m_animator.SetBool("north", moveDir.y >= 0.1f);
            m_animator.SetBool("south", moveDir.y <= -0.1f);
        }
    }

    private void Attack()
    {
        StartCoroutine(TryPlayAction("Attack", m_attackBuffer));
    }


    private void Dodge()
    {
        StartCoroutine(TryPlayAction("Dodge", m_dodgeBuffer));
    }
    
    private IEnumerator TryPlayAction(string _name, float _buffer)
    {
        if(m_hasControl) m_locomotion.SetTrigger(_name);
        yield return new WaitForSeconds(_buffer);
        if(m_hasControl) m_locomotion.ResetTrigger(_name);
    }
}

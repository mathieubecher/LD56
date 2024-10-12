using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Character : LivingHitable
{
    private Animator m_locomotion;
    
    [Header("Character")]
    [SerializeField] private Animator m_animator;
    [SerializeField] private ReceiveItem m_receiveItem;
    [SerializeField] private float m_attackBuffer = 0.2f;
    [SerializeField] private float m_dodgeBuffer = 0.2f;
    
    private float m_currentAttackBuffer;
    private float m_currentDodgeBuffer;

    public Animator animator => m_animator;
    public ReceiveItem receiveItem => m_receiveItem;
    public Life life => m_life;
    public int currentLife => m_life.currentLife;
    private bool hasControl => GameManager.hasControl;
    public Animator locomotion => m_locomotion;
    public Vector2 velocity
    {
        get => m_rigidbody.velocity;
        set => m_rigidbody.velocity = value;
    }

    [Header("BoxCast")] 
    [SerializeField] private Vector2 m_castBoxSize;
    [SerializeField] private Vector2 m_castOffset;
    [SerializeField] private float m_castDist;
    [SerializeField] private LayerMask m_castLayerMask;
    protected override void Awake()
    {
        base.Awake();
        m_locomotion = GetComponent<Animator>();
        this.gameObject.GetInstanceID();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Controller.OnAttackPress += Attack;
        Controller.OnDodgePress += Dodge;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Controller.OnAttackPress -= Attack;
        Controller.OnDodgePress -= Dodge;
    }

    protected override void Update()
    {
        base.Update();
        BufferManagment();
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (!hasControl) return;
        CheckCollision();
    }

    private void CheckCollision()
    {
        Vector2 direction = Controller.moveDir;
        if (Controller.tilt > 0.0f && (direction.x == 0.0f || direction.y == 0.0f))
        {
            Vector2 origin = (Vector2)transform.position + m_castOffset;
            Vector2 size = m_castBoxSize;
            float angle = 0.0f;
            float distance = m_castDist;
            RaycastHit2D[] cast = Physics2D.BoxCastAll(origin, size, angle, direction, distance, m_castLayerMask);
            if (cast.Length > 0)
            {
                foreach (var hit in cast)
                {
                    if(hit.collider.isTrigger) continue;
                    if(!m_animator.GetBool("push")) m_animator.SetTrigger("StartPush");
                    m_animator.SetBool("push", true);
                    return;
                }
            }
        }
        
        if(m_animator.GetBool("push")) m_animator.SetTrigger("Resume");
        m_animator.SetBool("push", false);
    }

    public void UpdateDirection()
    {
        if (!hasControl) return;
        
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
        if (hasControl && GameManager.HasItem("Sword"))
        {
            m_locomotion.SetTrigger("Attack");
            m_currentAttackBuffer = m_attackBuffer;
            m_currentDodgeBuffer = 0.0f;
        }
    }
    
    private void Dodge()
    {
        if (hasControl)
        {
            m_locomotion.SetTrigger("Dodge");
            m_currentDodgeBuffer = m_dodgeBuffer;
            m_currentAttackBuffer = 0.0f;
        }
    }

    protected override void OnDamaged(Vector2 _source, int _damage)
    {
        base.OnDamaged(_source, _damage);
        if (hasControl)
        {
            m_locomotion.SetTrigger("Hit");
            GameManager.frame.Shake();
        }
    }

    protected override void OnDead()
    {
        m_locomotion.SetBool("dead", true);
        GameManager.frame.Shake();
        StartCoroutine(Respawn());
        
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        GameManager.Play();
    }

}

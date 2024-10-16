using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Character : LivingHitable
{
    private Animator m_locomotion;
    
    [Header("Character")]
    [SerializeField] private ReceiveItem m_receiveItem;
    [SerializeField] private Grab m_grab;
    [SerializeField] private DetectCollision m_detect;
    [SerializeField] private float m_attackBuffer = 0.2f;
    [SerializeField] private float m_dodgeBuffer = 0.2f;
    [SerializeField] private float m_grabBuffer = 0.2f;
    
    private float m_currentAttackBuffer;
    private float m_currentDodgeBuffer;
    private float m_currentGrabBuffer;
    
    public bool hasControl => GameManager.hasControl;
    public ReceiveItem receiveItem => m_receiveItem;
    public Grab grab => m_grab;
    public DetectCollision detect => m_detect;
    public Life life => m_life;
    public int currentLife => m_life.currentLife;
    public Animator locomotion => m_locomotion;
    public Vector2 velocity
    {
        get => m_rigidbody.velocity;
        set => m_rigidbody.velocity = value;
    }

    protected override void Awake()
    {
        base.Awake();
        m_locomotion = GetComponent<Animator>();
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
        Push();
    }

    public void UpdateDirection()
    {
        if (!hasControl) return;
        
        Vector2 moveDir = Controller.lastValidDir;
        m_locomotion.SetBool("side", math.abs(moveDir.x) > 0.1f  && (m_locomotion.GetBool("side") || math.abs(moveDir.y) < 0.1f));
        m_locomotion.SetBool("north", moveDir.y >= 0.1f && (m_locomotion.GetBool("north") || math.abs(moveDir.x) < 0.1f));
        m_locomotion.SetBool("south", moveDir.y <= -0.1f && (m_locomotion.GetBool("south") || math.abs(moveDir.x) < 0.1f));

        if(m_locomotion.GetBool("side")) transform.localScale = new Vector3(math.sign(moveDir.x), 1.0f, 1.0f);
    }

    
    private void BufferManagment()
    {
        //TODO Create a system to manage that better
        if (m_currentAttackBuffer >= 0.0f)
        {
            m_currentAttackBuffer -= Time.deltaTime;
            if (m_currentAttackBuffer <= 0.0f)
                m_locomotion.ResetTrigger("Attack");
        }
        if (m_currentDodgeBuffer >= 0.0f)
        {
            m_currentDodgeBuffer -= Time.deltaTime;
            if (m_currentDodgeBuffer <= 0.0f)
                m_locomotion.ResetTrigger("Dodge");
        }
        if (m_currentGrabBuffer >= 0.0f)
        {
            m_currentGrabBuffer -= Time.deltaTime;
            if (m_currentGrabBuffer <= 0.0f)
                m_locomotion.ResetTrigger("Grab");
        }
    }
    private void Attack()
    {
        if (hasControl && GameManager.HasItem("Sword"))
        {
            m_locomotion.SetTrigger("Attack");
            m_currentAttackBuffer = m_attackBuffer;
            m_currentDodgeBuffer = 0.0f;
            m_currentGrabBuffer = 0.0f;
        }
    }
    
    private void Dodge()
    {
        if (hasControl)
        {
            m_locomotion.SetTrigger("Dodge");
            m_currentDodgeBuffer = m_dodgeBuffer;
            m_currentAttackBuffer = 0.0f;
            m_currentGrabBuffer = 0.0f;
        }
    }
    
    public void Grab()
    {
        m_locomotion.SetTrigger("Grab");
        locomotion.SetBool("grab", true);
        m_currentGrabBuffer = m_grabBuffer;
        m_currentAttackBuffer = 0.0f;
        m_currentDodgeBuffer = 0.0f;
    }

    public void Launch()
    {
        m_locomotion.SetTrigger("Launch");
        locomotion.SetBool("grab", false);
        m_currentGrabBuffer = m_grabBuffer;
        m_currentAttackBuffer = 0.0f;
        m_currentDodgeBuffer = 0.0f;
        
    }
    
    private void Push()
    {
        //TODO Place on specific monobehaviour
        Vector2 direction = Controller.moveDir;
        bool grab = m_locomotion.GetBool("grab");
        bool isContact = !grab && m_detect.isContact && Controller.tilt > 0.0f && (direction.x == 0.0f || direction.y == 0.0f);
        
        if(!m_locomotion.GetBool("push") && isContact) m_locomotion.SetTrigger("StartPush");
        else if(m_locomotion.GetBool("push") && !grab && !isContact) m_locomotion.SetTrigger("Resume");
        
        m_locomotion.SetBool("push", isContact);
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

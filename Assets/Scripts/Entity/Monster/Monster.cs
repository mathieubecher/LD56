using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Monster : LivingHitable
{
    [Header("Monster")]
    [SerializeField] private Animator m_animator;
    [SerializeField] private Loot m_loot;
    

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (m_life.dead) return;
        if (m_hitPushTimer <= 0.0f)
        {
            Move();
            UpdateDirection();
        } 
    }

    protected virtual void Move()
    {
        
    }
    protected override void OnDamaged(Vector2 _source, int _damage)
    {
        base.OnDamaged(_source, _damage);
        m_animator.SetBool("move", false);
        m_animator.SetTrigger("Hit");
    }
    protected override void OnDead()
    {
        m_animator.SetBool("move", false);
        m_animator.SetBool("dead", true);
        StartCoroutine(DeadDestroy());
    }

    private IEnumerator DeadDestroy()
    {
        yield return new WaitForSecondsRealtime(1.15f);
        m_loot.CreateLoot();
        Destroy(gameObject);
    }

    public virtual void UpdateDirection()
    {
        if (!GameManager.hasControl) return;
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

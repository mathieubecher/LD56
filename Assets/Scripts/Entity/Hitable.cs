using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitable : MonoBehaviour
{
    [Header("Hitable")]
    [SerializeField] private Hurtbox m_hurtbox;
    public Hurtbox hurtbox => m_hurtbox;
    protected virtual void Awake()
    {
        
    }
    
    protected virtual void OnEnable()
    {
        m_hurtbox.OnHit += Hit;
    }

    protected virtual void OnDisable()
    {
        m_hurtbox.OnHit -= Hit;
    }
    
    public virtual void Hit(Vector2 _source, int _damage)
    {
        
    }
    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        
    }

}

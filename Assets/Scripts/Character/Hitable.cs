using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitable : MonoBehaviour
{
    [Header("Hitable")]
    [SerializeField] private Hurtbox m_hurtbox;
    
    protected void OnEnable()
    {
        m_hurtbox.OnHit += Hit;
    }

    protected void OnDisable()
    {
        m_hurtbox.OnHit -= Hit;
    }
    
    protected virtual void Hit(Vector2 _source, int _damage)
    {
        
    }

}

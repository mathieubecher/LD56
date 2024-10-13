using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    [Header("GrabObject")]
    [SerializeField] private Transform m_shadow;
    [SerializeField] private Collider2D m_physic;
    [SerializeField] private Breakable m_breakable;
    
    public void Grab()
    {
        if(m_breakable) m_breakable.hurtbox.gameObject.SetActive(false);
        m_shadow.gameObject.SetActive(false);
        m_physic.enabled = false;
    }
    
    public void Release(Vector2 _pos)
    {
        m_shadow.gameObject.SetActive(true);
        transform.position = _pos;
        if(m_breakable) m_breakable.Hit(_pos, 0);
        //TODO Destroy object

    }
}

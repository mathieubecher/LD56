using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    [Header("GrabObject")]
    [SerializeField] private Transform m_shadow;
    [SerializeField] private Collider2D m_physic;
    [SerializeField] private Hurtbox m_hurtbox;
    
    public void Grab()
    {
        m_hurtbox.gameObject.SetActive(false);
        m_shadow.gameObject.SetActive(false);
        m_physic.enabled = false;
    }
    
    public void Release()
    {
        m_hurtbox.gameObject.SetActive(true);
        m_shadow.gameObject.SetActive(true);
        m_physic.enabled = true;
        
    }
}

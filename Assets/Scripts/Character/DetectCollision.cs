using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    
    [Header("Box Cast")] 
    [SerializeField] private Vector2 m_castBoxSize;
    [SerializeField] private Vector2 m_castOffset;
    [SerializeField] private float m_castDist;
    [SerializeField] private float m_contactDist;
    [SerializeField] private LayerMask m_castLayerMask;
    [Header("Check Case")]
    [SerializeField] private float m_checkCaseRadius;
    [Header("Debug")]
    [SerializeField] private Collider2D m_contact;
    [SerializeField] private float m_nearDistance;

    public bool isContact => m_contact && m_nearDistance <= m_contactDist;
    public bool isWearable => m_contact && m_contact.CompareTag("Wearable");
    public GrabObject grab => m_contact ? m_contact.GetComponent<GrabObject>() : null;

    public Collider2D contact => m_contact;
    public float nearDistance => m_nearDistance;
    
    private void FixedUpdate()
    {
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
                int nearest = -1;
                for (int i = 0; i < cast.Length; ++i)
                {
                    if(cast[i].collider.isTrigger) continue;
                    if (nearest < 0 || cast[nearest].distance > cast[i].distance)
                    {
                        nearest = i;
                    }
                }

                if (nearest >= 0)
                {
                    m_contact = cast[nearest].collider;
                    m_nearDistance = cast[nearest].distance;
                    return;
                }
            }
            m_contact = null;
            m_nearDistance = -1.0f;
        }
    }

    public bool canPlaceAt(Vector2 _pos)
    {
        Vector2 origin = _pos;
        float size = m_checkCaseRadius;
        
        RaycastHit2D[] cast = Physics2D.CircleCastAll(origin, size, Vector2.zero, 0.0f);
        if (cast.Length > 0)
        {
            for (int i = 0; i < cast.Length; ++i)
            {
                if(cast[i].collider.isTrigger) continue;
                return false;
            }
        }

        return true;
    }
}

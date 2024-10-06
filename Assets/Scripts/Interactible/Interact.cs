using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private Interactible m_contact;
    private void OnEnable()
    {
        Controller.OnInteractPress += Press;
    }

    private void OnDisable()
    {
        Controller.OnInteractPress -= Press;
        
    }

    private void Press()
    {
        if (GameManager.instance.hasControl)
        {
            if (m_contact)
            {
                m_contact.Interact();
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.TryGetComponent(out Interactible _interactible))
        {
            m_contact = _interactible;
        }
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (m_contact && m_contact.gameObject == _other.gameObject)
        {
            m_contact = null;
        }
    }
}

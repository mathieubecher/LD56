using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private bool m_infinite = true;
    [SerializeReference, SubclassSelector] private InteractModule[] m_modules = Array.Empty<InteractModule>();
    private bool m_activated;

    public void Awake()
    {
        foreach (InteractModule module in m_modules)
        {
            module.Awake(this);
        }
    }
    public void Interact()
    {
        if (m_activated && !m_infinite) return;
        
        foreach (InteractModule module in m_modules)
        {
            if (!module.Activate(this)) return;
        }
        m_activated = true;
        Activate();
        PlayEffect();
    }

    public virtual void Activate()
    {
        
    }

    protected virtual void PlayEffect()
    {
        
    }
}

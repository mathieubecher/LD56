using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Persistent : InteractModule
{
    [SerializeField] private string m_name;
    public void Awake(Interactable _interactable)
    {
        if (GameManager.HasPersistent(m_name))
        {
            _interactable.Activate();   
        }
    }

    public bool Activate(Interactable _interactable)
    {
        GameManager.AddPersistent(m_name);
        return true;
    }
}

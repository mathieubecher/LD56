using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class RequireItem : InteractModule
{
    [SerializeField] private string m_item;
    [SerializeField] private int m_value = 1;
    [SerializeField] private Cinematic m_fail;

    public void Awake(Interactable _interactable)
    {
    }

    public bool Activate(Interactable _interactable)
    {
        if (GameManager.NumberItem(m_item) >= m_value)
        {
            GameManager.UseItem(m_item, m_value);
            return true;
        }
        
        CinematicManager.instance.Play(m_fail);
        return false;
    }
}

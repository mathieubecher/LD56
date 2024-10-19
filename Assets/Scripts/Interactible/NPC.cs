using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    [SerializeField] private Cinematic m_interact;
    protected override void PlayEffect()
    {
        CinematicManager.instance.Play(m_interact);
    }
}

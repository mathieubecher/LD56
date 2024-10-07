using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactible
{
    [SerializeField] private Cinematic m_interact;
    public override void Interact()
    {
        CinematicManager.instance.Play(m_interact);
    }
}

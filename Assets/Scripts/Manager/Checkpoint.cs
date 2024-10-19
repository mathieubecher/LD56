using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : Interactable
{
    [SerializeField] private string m_name;
    public string name => m_name;
    
    public override void Activate()
    {
        GameManager.SaveCheckpoint(this);
    }

    protected override void PlayEffect()
    {
        
    }
}

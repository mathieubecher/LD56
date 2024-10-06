using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactible
{
    public override void Interact()
    {
        GameManager.instance.frame.StartDialog(3);
    }
}

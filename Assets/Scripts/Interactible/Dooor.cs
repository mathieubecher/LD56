using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dooor : Interactable
{
    protected override void PlayEffect()
    {
    }

    public override void Activate()
    {
        Destroy(gameObject);
    }
}

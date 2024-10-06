using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dooor : Interactible
{
    public override void Interact()
    {
        if (GameManager.HasItem("Key"))
        {
            GameManager.UseItem("Key", 1);
            Destroy(gameObject);
        }
        else
        {
            GameManager.frame.StartDialog(3);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dooor : Interactible
{
    [SerializeField] private Cinematic m_fail;
    public override void Interact()
    {
        if (GameManager.HasItem("Key"))
        {
            GameManager.UseItem("Key", 1);
            Destroy(gameObject);
        }
        else
        {
            CinematicManager.instance.Play(m_fail);
        }
    }
}

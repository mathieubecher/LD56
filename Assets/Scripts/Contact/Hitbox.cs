using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D _other)
    {
        //Debug.Log(this.gameObject.name + " - " + this.tag + " -> " + _other.gameObject.name + " - " + _other.tag);
        if (GameManager.HitRelation(this.tag, _other.tag))
        {
            _other.GetComponent<Hurtbox>().Hit(transform.position, 1);
        }
    }
}

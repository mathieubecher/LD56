using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public delegate void HitEvent(Vector2 _source, int _damage);
    public event HitEvent OnHit;

    public void Hit(Vector2 _source, int _damage)
    {
        OnHit?.Invoke(_source, _damage);
        
    }
}

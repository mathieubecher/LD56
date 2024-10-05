using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public delegate void HitEvent(Vector2 _source, float _damage);
    public static event HitEvent OnHit;

    public void Hit(Vector2 _source, float _damage)
    {
        Debug.Log("Hit");
        OnHit?.Invoke(_source, _damage);
        
    }
}

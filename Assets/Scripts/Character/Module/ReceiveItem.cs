using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveItem : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private SpriteRenderer m_receive;
    
    protected void OnEnable()
    {
        GameManager.OnResume += Resume;
    }
    
    protected void OnDisable()
    {
        GameManager.OnResume -= Resume;
    }
    
    public void Receive(ItemSprite _item)
    {
        m_receive.sprite = _item.sprite;
        m_animator.SetTrigger("Receive");
    }
    
    public void Resume()
    {
        m_animator.SetTrigger("Resume");
    }
}

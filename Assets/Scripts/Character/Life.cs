using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    
    [SerializeField] private int m_maxLife = 3;
    private int m_currentLife;

    public int currentLife { get => m_currentLife; }
    public bool dead => m_currentLife == 0;

    private void Awake()
    {
        m_currentLife = m_maxLife;
    }
    public bool Hit(int _damage)
    {
        if (dead) return false;
        
        m_currentLife -= _damage;
        if (m_currentLife <= 0)
        {
            m_currentLife = 0;
        }
        return true;
    }
}

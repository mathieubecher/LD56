using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    #region Singleton
    
    private static LevelManager m_instance;
    public static LevelManager instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindAnyObjectByType<LevelManager>();
            }
            return m_instance;
        }
    }
    #endregion
    [SerializeField] private Frame m_frame;
    [SerializeField] private Character m_character;
    public static Frame frame => instance.m_frame;
    public static Character character => instance.m_character;
}

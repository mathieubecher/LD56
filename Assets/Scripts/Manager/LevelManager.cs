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
    [SerializeField] private float m_cellSize = 0.5f;
    [SerializeField] private Checkpoint m_defaultCheckpoint;
    [SerializeField] private List<Checkpoint> m_checkpoints;
    public static Frame frame => instance.m_frame;
    public static Character character => instance.m_character;
    public static float cellSize => instance.m_cellSize;
    public static List<Checkpoint> checkpoints => instance.m_checkpoints;

    public static Checkpoint GetCheckpoint(string _name)
    {
        if (_name == "") return instance.m_defaultCheckpoint;
        return checkpoints.Find(x => x.name == _name);
    }
}

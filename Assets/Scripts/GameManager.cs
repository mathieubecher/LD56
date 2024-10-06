using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }
    #endregion

    private bool m_hasControl = true;
    private List<string> m_items;
    [SerializeField] private Frame m_frame;
    [SerializeField] private Character m_character;
    public bool hasControl => m_hasControl;
    public static Frame frame => instance.m_frame;
    public static Character character => instance.m_character;
    
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Manager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        m_items = new List<string>();
    }
    
    public void Play()
    {
        SceneManager.LoadScene("Level");
    }
    
    public void Exit()
    {
        SceneManager.LoadScene("Main");
    }
    
    public void Result()
    {
        SceneManager.LoadScene("Result");
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        m_hasControl = false;
    }
    public void Resume()
    {
        Time.timeScale = 1.0f;
        m_hasControl = true;
    }

    public static bool HitRelation(string _hitbox, string _hurtbox)
    {
        return _hitbox != _hurtbox;
    }

    public static bool HadItem(string _item)
    {
        if (instance.m_items == null) return false;
        return instance.m_items.Contains(_item);
    }

    public static void GiveItem(string _item)
    {
        if (!HadItem(_item))
        {
            instance.m_items.Add(_item);
        }
    }

    public static void PickItem(string _item)
    {
        switch (_item)
        {
            case "Heal":
                character.life.Heal(1);
                break;
        }
    }
}

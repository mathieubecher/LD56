using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct ItemSprite
{
    public string item;
    public Sprite sprite;
}
public class GameManager : MonoBehaviour
{
    #region Singleton
    
    public delegate void SimpleEvent();
    public static event SimpleEvent OnPause;
    public static event SimpleEvent OnResume;
    
    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindAnyObjectByType<GameManager>();
            }
            return m_instance;
        }
    }
    #endregion

    [SerializeField] private List<ItemSprite> m_itemsSprites;
    [SerializeField] private bool m_ignoreCinematic;
    
    private bool m_hasControl = true;
    private Dictionary<string, int> m_items;
    private List<string> m_persistents;
    public static bool hasControl => instance.m_hasControl;
    public static bool ignoreCinematic => instance.m_ignoreCinematic;
    public static Frame frame => LevelManager.frame;
    public static Character character => LevelManager.character;
    public static float cellSize => LevelManager.cellSize;
    
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Manager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        m_items = new Dictionary<string, int>();
        m_persistents = new List<string>();
    }

    public static void Play()
    {
        SceneManager.LoadScene("Level");
    }
    
    public static void Exit()
    {
        SceneManager.LoadScene("Main");
    }
    
    public static void Result()
    {
        SceneManager.LoadScene("Result");
    }

    public static void Pause()
    {
        Time.timeScale = 0.0f;
        instance.m_hasControl = false;
        OnPause?.Invoke();
    }
    public static void Resume()
    {
        Time.timeScale = 1.0f;
        instance.m_hasControl = true;
        OnResume?.Invoke();
    }

    public static void TakeControl()
    {
        Pause();
        instance.m_hasControl = false;
    }

    public static void GiveControl()
    {
        Resume();
        instance.m_hasControl = true;
    }
    
    public static bool HitRelation(string _hitbox, string _hurtbox)
    {
        return _hitbox != _hurtbox;
    }

    public static bool HasItem(string _item)
    {
        return NumberItem(_item) > 0;
    }
    
    public static int NumberItem(string _item)
    {
        if (instance.m_items == null || !instance.m_items.ContainsKey(_item)) return 0;
        return instance.m_items[_item];
    }
    public static void UseItem(string _item, int _number)
    {
        instance.m_items[_item] -= _number;
    }

    public static void AddItem(string _item, int _number)
    {
        if (!HasItem(_item))
        {
            instance.m_items.Add(_item, _number);
        }
        else
        {
            ++instance.m_items[_item];
        }
    }

    public static ItemSprite GetItemSprite(string _item)
    {
        return instance.m_itemsSprites.Find(x => x.item == _item);
    }
    
    public static void PickItem(string _item)
    {
        switch (_item)
        {
            case "Heal":
                character.life.Heal(1);
                break;
            case "Coin":
                AddItem(_item, 1);
                break;
        }
    }

    public static bool HasPersistant(string _persistent)
    {
        if (instance.m_persistents == null) return false;
        return instance.m_persistents.Contains(_persistent);
    }

    public static void AddPersistant(string _persistent)
    {
        if (HasPersistant(_persistent)) return;
        instance.m_persistents.Add(_persistent);
    }

}

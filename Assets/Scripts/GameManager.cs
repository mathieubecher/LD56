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
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }
    #endregion

    private bool m_hasControl = true;
    private Dictionary<string, int> m_items;
    [SerializeField] private List<ItemSprite> m_itemsSprites;
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
        m_items = new Dictionary<string, int>();
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
        OnPause?.Invoke();
    }
    public void Resume()
    {
        Time.timeScale = 1.0f;
        m_hasControl = true;
        OnResume?.Invoke();
    }

    public static bool HitRelation(string _hitbox, string _hurtbox)
    {
        return _hitbox != _hurtbox;
    }

    public static bool HasItem(string _item)
    {
        if (instance.m_items == null) return false;
        return instance.m_items.ContainsKey(_item);
    }

    public static void GiveItem(string _item)
    {
        AddItem(_item, 1);
        instance.StartCoroutine(instance.GiveItemCinematic(_item));
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

    private IEnumerator GiveItemCinematic(string _item)
    {
        Pause();
        character.ReceiveItem(m_itemsSprites.Find(x => x.item == _item));
        yield return new WaitForSecondsRealtime(2.0f);
        frame.StartDialog(3);
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
}

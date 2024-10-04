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
    
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Manager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
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
        
    }
}

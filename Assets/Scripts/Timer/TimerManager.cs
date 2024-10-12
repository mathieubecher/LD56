using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    #region Singleton
    
    private static TimerManager m_instance;
    public static TimerManager instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindAnyObjectByType<TimerManager>();
            }
            return m_instance;
        }
    }
    #endregion

    private List<GameTimer> m_timers;

    private void OnEnable()
    {
        GameManager.OnResume += Resume;
        GameManager.OnPause += Pause;
    }

    private void OnDisable()
    {
        GameManager.OnResume -= Resume;
        GameManager.OnPause -= Pause;
    }

    private void Resume()
    {
        /* foreach (GameTimer timer in m_timers)
        {
            timer.Resume();
        } */
    }

    private void Pause()
    {
        /* foreach (GameTimer timer in m_timers)
        {
            timer.Stop();
        } */
    }
    
    public static GameTimer CreateTimer()
    {
        GameTimer newTimer = new GameTimer();
        instance.m_timers.Add(newTimer);
        
        return newTimer;
    }
}

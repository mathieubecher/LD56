using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void Play()
    {
        GameManager.Play();
    }
    
    public void Exit()
    {
        GameManager.Exit();
    }

    public void Pause()
    {
        GameManager.Pause();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void Play()
    {
        GameManager.instance.Play();
    }
    
    public void Exit()
    {
        GameManager.instance.Exit();
    }

    public void Pause()
    {
        GameManager.instance.Pause();
    }
}

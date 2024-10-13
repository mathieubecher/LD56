using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private Character m_character;
    
    
    protected void OnEnable()
    {
        Controller.OnGrabPress += InputPress;
    }

    protected void OnDisable()
    {
        Controller.OnGrabPress -= InputPress;
    }
    
    private void InputPress()
    {
        if (m_character.hasControl)
        {
            bool isGrabbing = m_character.locomotion.GetBool("grab");
            
            if(!isGrabbing) m_character.locomotion.SetTrigger("Grab");
            else m_character.locomotion.SetTrigger("Launch");
            
            m_character.locomotion.SetBool("grab", !isGrabbing);
        }
    }

    public void LaunchObject()
    {
        ReleaseObject(); // TODO create action
    }
    
    public void ReleaseObject()
    {
        
    }

}

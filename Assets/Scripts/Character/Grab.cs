using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private Character m_character;
    [SerializeField] private Transform m_grabPoint;

    private GrabObject m_grab;
    
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
            Debug.Log(m_character.detect.grab);
            bool isGrabbing = m_character.locomotion.GetBool("grab");
            
            if(!isGrabbing && m_character.detect.grab) m_character.locomotion.SetTrigger("Grab");
            else if(isGrabbing) m_character.locomotion.SetTrigger("Launch");
            
            m_character.locomotion.SetBool("grab", !isGrabbing);
        }
    }

    public void GrabObject(GrabObject _grab)
    {
        if (!_grab) return; 
        m_grab = _grab;
        m_grab.Grab();
        m_grab.transform.parent = m_grabPoint;
        m_grab.transform.localPosition = Vector3.zero;
        Debug.Log("Grab");
    }

    public void LaunchObject()
    {
        ReleaseObject(); // TODO create action
    }
    
    public void ReleaseObject()
    {
        if (!m_grab) return;
        m_grab.Release();
        m_grab.transform.parent = null;
        m_grab = null;
        Debug.Log("Release");
    }

}

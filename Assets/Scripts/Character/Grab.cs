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
            
            if(!isGrabbing && m_character.detect.grab) m_character.Grab();
            else if(isGrabbing) m_character.Launch();
            
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
    }

    public void LaunchObject(Vector2 _pos)
    {
        ReleaseObject(_pos); //TODO create action
    }
    
    public void ReleaseObject(Vector2 _pos)
    {
        if (!m_grab) return;
        m_grab.Release(_pos);
        m_grab.transform.parent = null;
        m_grab = null;
    }

}

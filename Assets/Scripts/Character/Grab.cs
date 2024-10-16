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
            bool isGrabbing = m_character.locomotion.GetBool("grab");
            
            if(!isGrabbing && m_character.detect.grab) m_character.Grab();
            else if(isGrabbing) m_character.Launch();
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

    public void ReleaseObject(Vector2 _pos, Vector2? _direction = null, float _distance = 0.0f)
    {
        if (!m_grab) return;
        m_grab.Release(_pos, _direction, _distance);
        
        m_character.locomotion.SetBool("grab", false);
        m_grab = null;
    }

}

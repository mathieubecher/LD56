using UnityEngine;

public class LocomotionStateGrab : StateMachineBehaviour
{
    private Character m_character;
    override public void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        //Debug.Log("Enter Dodge State");
        if(!m_character) m_character = _animator.GetComponent<Character>();
        m_character.grab.GrabObject(m_character.detect.grab);
        m_character.velocity = Vector2.zero;
        _animator.SetBool("canAction", false);
    }
    
    override public void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
    }

    override public void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        _animator.SetBool("canAction", true);
        m_character.UpdateDirection();
    }
}

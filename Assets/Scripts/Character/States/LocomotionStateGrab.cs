using UnityEngine;

public class LocomotionStateGrab : StateMachineBehaviour
{
    private Character m_character;
    override public void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        //Debug.Log("Enter Dodge State");
        if(!m_character) m_character = _animator.GetComponent<Character>();
        m_character.grab.GrabObject(m_character.detect.grab);
    }
    
    override public void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
    }

    override public void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
    }
    
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
    }

    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
    }
}

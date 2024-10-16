using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionStateHit : StateMachineBehaviour
{
    private Character m_character;
    
    override public void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        m_character = _animator.GetComponent<Character>();
        m_character.grab.ReleaseObject(m_character.transform,m_character.transform.position);

        _animator.SetBool("canAction", false);
    }
    
    override public void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
    
    }

    override public void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        _animator.SetBool("canAction", true);
    }
}

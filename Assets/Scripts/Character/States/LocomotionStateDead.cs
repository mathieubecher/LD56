using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionStateDead : StateMachineBehaviour
{
    private Character m_character;
    
    override public void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        if(!m_character) m_character = _animator.GetComponent<Character>();
        m_character.animator.SetBool("dead", true);
    }
    
    override public void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        
    }

    override public void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        _animator.SetBool("canDodge", true);
        _animator.SetBool("canAttack", true);
        m_character.animator.SetBool("dead", false);
    }
}

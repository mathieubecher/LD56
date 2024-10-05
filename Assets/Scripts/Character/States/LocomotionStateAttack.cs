using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionStateAttack : StateMachineBehaviour
{
    private Character m_character;
    
    override public void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        m_character = _animator.GetComponent<Character>();
        _animator.SetBool("canDodge", false);
        _animator.SetBool("canAttack", false);
        
        m_character.animator.SetTrigger("Attack");
        m_character.velocity = Vector2.zero;  
    }
    
    override public void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        
    }

    override public void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        _animator.SetBool("canDodge", true);
        _animator.SetBool("canAttack", true);
    }
}

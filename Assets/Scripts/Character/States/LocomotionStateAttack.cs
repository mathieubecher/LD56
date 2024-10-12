using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionStateAttack : StateMachineBehaviour
{
    [SerializeField] private float m_canCancelAttack = 0.2f;
    
    private Character m_character;
    private float m_timer;

    override public void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        //Debug.Log("Enter Attack State");
        if(!m_character) m_character = _animator.GetComponent<Character>();
        _animator.SetBool("canAction", false);
        
        m_character.UpdateDirection();
        m_character.velocity = Vector2.zero;  
        m_timer = 0.0f;
    }
    
    override public void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        m_timer += Time.deltaTime;
        if (m_timer >= m_canCancelAttack)
        {
            _animator.SetBool("canAction", true);
        }
        
    }

    override public void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        _animator.SetBool("canAction", true);
    }
    
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("Enter Attack SubState");
    }

    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("Exit Attack SubState");
    }
}

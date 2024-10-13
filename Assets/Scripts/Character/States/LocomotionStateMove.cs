using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionStateMove : StateMachineBehaviour
{
    [SerializeField] private float m_speed = 1.0f;
    private Character m_character;
    
    override public void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        //Debug.Log("Enter Move State");
        if(!m_character) m_character = _animator.GetComponent<Character>();
    }
    
    override public void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {

        if (!GameManager.hasControl)
        {
            _animator.SetBool("move", false);
            return;
        }
        m_character.UpdateDirection();
        m_character.velocity = Controller.moveDir * Controller.tilt * m_speed;
        _animator.SetBool("move", Controller.tilt > 0.1f);
    }
    
    override public void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        //m_character.animator.SetBool("move", false);
    }
    
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        //Debug.Log("Enter Move SubState");
    }

    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        //Debug.Log("Exit Move SubState");
    }
}

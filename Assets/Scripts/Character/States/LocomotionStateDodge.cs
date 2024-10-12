using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionStateDodge : StateMachineBehaviour
{
    [SerializeField] private float m_dodgeSpeed = 3.0f;
    [SerializeField] private AnimationCurve m_speedOverTime;
    private Character m_character;
    private Vector2 m_dodgeDirection;
    private float m_timer;
    
    override public void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        //Debug.Log("Enter Dodge State");
        if(!m_character) m_character = _animator.GetComponent<Character>();
        _animator.SetBool("canAction", false);
        m_dodgeDirection = Controller.lastValidDir;
        
        m_character.UpdateDirection();
        m_timer = 0.0f;
    }
    
    override public void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        m_timer += Time.deltaTime;
        m_character.velocity = m_dodgeDirection * m_dodgeSpeed * m_speedOverTime.Evaluate(m_timer);
    }

    override public void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        _animator.SetBool("canAction", true);
    }
    
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("Enter Dodge SubState");
    }

    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("Exit Dodge SubState");
    }
}

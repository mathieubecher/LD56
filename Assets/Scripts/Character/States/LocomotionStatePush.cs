using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionStatePush : StateMachineBehaviour
{
    [SerializeField] private AnimationCurve m_positionOverTime;
    private Character m_character;
    private Vector2 m_pushDirection;
    private float m_timer;
    
    override public void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        //Debug.Log("Enter Push State");
        if(!m_character) m_character = _animator.GetComponent<Character>();
        _animator.SetBool("canAction", false);
        m_pushDirection = Controller.lastValidDir;
        
        m_character.UpdateDirection();
        m_timer = 0.0f;
    }
    
    override public void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        m_timer += Time.deltaTime;
        m_character.velocity = m_pushDirection * m_positionOverTime.Evaluate(m_timer);
    }

    override public void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        _animator.SetBool("canAction", true);
    }
    
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("Enter Push SubState");
    }

    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("Exit Push SubState");
    }
}

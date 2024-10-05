using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionStateMove : StateMachineBehaviour
{
    [SerializeField] private float m_speed = 1.0f;
    private Character m_character;
    
    override public void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        m_character = _animator.GetComponent<Character>();
    }
    
    override public void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
    
        m_character.velocity = Controller.moveDir * Controller.tilt * m_speed;    
    }
    
    override public void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        
    }
}

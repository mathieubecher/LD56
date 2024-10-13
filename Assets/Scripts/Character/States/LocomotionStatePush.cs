using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LocomotionStatePush : StateMachineBehaviour
{
    [SerializeField] private AnimationCurve m_positionOverTime;
    [SerializeField] private float m_duration;
    
    private Character m_character;
    private Vector2 m_pushDirection;
    private float m_timer;
    private float m_currentPos;
    private Transform m_pushObject;
    
    override public void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        //Debug.Log("Enter Push State");
        if(!m_character) m_character = _animator.GetComponent<Character>();
        _animator.SetBool("canAction", false);
        m_pushDirection = Controller.lastValidDir;
        
        m_character.UpdateDirection();
        NextMove();
    }
    
    private void NextMove()
    {
        ResetObjectPos();
        m_timer = 0.0f;
        m_currentPos = 0.0f;
        m_character.velocity = Vector2.zero;
        if(m_character.detect.isContact && m_character.detect.isWearable)
        {
            Vector2 nextPos = (Vector2)m_character.detect.contact.transform.position + m_pushDirection * GameManager.cellSize;
            if (!m_character.detect.canPlaceAt(nextPos)) return;
            
            m_pushObject = m_character.detect.contact.transform;
            m_pushObject.parent = m_character.transform;
        }
    }
    
    override public void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        _animator.SetBool("canAction", m_character.velocity.magnitude == 0.0f);
        if (m_timer > 1.0f)
        {
            NextMove();
        }
        
        m_timer += Time.deltaTime / m_duration;
        float nextPos = m_positionOverTime.Evaluate(m_timer) * GameManager.cellSize;
        m_character.velocity = m_pushDirection * (nextPos - m_currentPos)/Time.deltaTime;
        m_currentPos = nextPos;
    }

    private void ResetObjectPos()
    {
        if (m_pushObject)
        {
            Vector2 finalPos = m_pushObject.position / GameManager.cellSize;
            finalPos = new Vector2(math.floor(finalPos.x), math.floor(finalPos.y)) * GameManager.cellSize + Vector2.one * GameManager.cellSize/2.0f;
            
            m_pushObject.parent = null;
            m_pushObject.position = finalPos;
            m_pushObject = null;
        }
    }
    
    override public void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        ResetObjectPos();
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

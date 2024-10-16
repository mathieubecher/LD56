using UnityEngine;

public class LocomotionStateLaunch : StateMachineBehaviour
{
    [SerializeField] private float m_launchDist;
    private Character m_character;
    override public void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        if(!m_character) m_character = _animator.GetComponent<Character>();
        m_character.grab.ReleaseObject(m_character.transform.position, Controller.lastValidDir, m_launchDist);
        m_character.velocity = Vector2.zero;
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

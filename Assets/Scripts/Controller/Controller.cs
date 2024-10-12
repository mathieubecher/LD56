using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    #region Singleton
    private static Controller m_instance;
    public static Controller instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindAnyObjectByType<Controller>();
            }
            return m_instance;
        }
    }
    public static Vector2 moveDir => instance.m_moveDir;
    public static Vector2 lastValidDir => instance.m_lastValidDir;
    public static float tilt => instance.m_tilt;
    #endregion

    [SerializeField] private AnimationCurve m_deadzone;
    [SerializeField] private AnimationCurve m_axis;
    private Vector2 m_moveDir;
    private Vector2 m_lastValidDir;
    private float m_tilt;

    public delegate void SimpleEvent();
    public static event SimpleEvent OnDodgePress;
    public static event SimpleEvent OnDodgeRelease;
    
    public static event SimpleEvent OnInteractPress;
    public static event SimpleEvent OnInteractRelease;
    
    public static event SimpleEvent OnAttackPress;
    public static event SimpleEvent OnAttackRelease;
    
    public static event SimpleEvent OnResetPress;
    public static event SimpleEvent OnContinuePress;
    public static event SimpleEvent OnPausePress;
    
    public void ReadMoveInput(InputAction.CallbackContext _context)
    {
        Vector2 input = _context.ReadValue<Vector2>();
        m_tilt = m_deadzone.Evaluate(input.magnitude);
        if (m_tilt <= 0.01f)
        {
            m_tilt = 0f;
            m_moveDir = Vector2.zero;
        }
        else
        {
            m_moveDir = input.normalized;
            m_moveDir = new Vector2(m_axis.Evaluate(m_moveDir.x), m_axis.Evaluate(m_moveDir.y));
            m_lastValidDir = m_moveDir;
        }
    }
    
    public void ReadDodgeInput(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            OnDodgePress?.Invoke();
        }
        else if (_context.canceled)
        {
            OnDodgeRelease?.Invoke();
        }
    }
    
    public void ReadInteractInput(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            OnInteractPress?.Invoke();
        }
        else if (_context.canceled)
        {
            OnInteractRelease?.Invoke();
        }
    }
    
    public void ReadAttackInput(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            OnAttackPress?.Invoke();
        }
        else if (_context.canceled)
        {
            OnAttackRelease?.Invoke();
        }
    }
    
    public void ReadResetInput(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            OnResetPress?.Invoke();
        }
    }
    
    public void ReadContinueInput(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            OnContinuePress?.Invoke();
        }
    }
    
    public void ReadPauseInput(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            OnPausePress?.Invoke();
        }
    }
}

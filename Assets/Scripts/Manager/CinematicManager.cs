using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;


[Serializable]
public struct Cinematic
{
    public List<String> sequence;
    public List<CinemachineVirtualCamera> cameras;
}

public class CinematicManager : MonoBehaviour
{
    #region Singleton
    
    private static CinematicManager m_instance;
    public static CinematicManager instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindAnyObjectByType<CinematicManager>();
            }
            return m_instance;
        }
    }
    #endregion
    
    [SerializeField] private CinemachineBrain m_camera;

    private bool m_active;
    private int m_request;
    private int m_currentAction;

    private Cinematic m_currentCinematic;
    
    public void Play(Cinematic _cinematic)
    {
        if (_cinematic.sequence.Count == 0) return;
        
        m_active = true;
        GameManager.TakeControl();

        if (GameManager.ignoreCinematic)
        {
            End();
            return;
        }

        m_currentAction = 0;
        m_currentCinematic = _cinematic;
        ReadAction(m_currentCinematic.sequence[m_currentAction]);
    }

    public void End()
    {
        m_active = false;
        //GameManager.currentCheckpoint.Activate(false);
        GameManager.GiveControl();
        GameManager.character.locomotion.enabled = true;
        foreach (var camera in m_currentCinematic.cameras)
        {
            camera.Priority = 0;
        }
    }

    private void Update()
    {
        if (m_active && m_currentAction < m_currentCinematic.sequence.Count) ReadNextAction();
    }

    private void EndRequest()
    {
        --m_request;
        //Debug.Log("End request " + m_request);
    }
    private void ReadNextAction()
    {
        if (m_request > 0) return;
        
        ++m_currentAction;
        m_request = 0;
        if (m_currentAction < m_currentCinematic.sequence.Count) ReadAction(m_currentCinematic.sequence[m_currentAction]);
        else End();
    }

    void ReadAction(String _action)
    {
        //Debug.Log(_action);
        String[] splitActions = _action.Split(new char[]{'[',']','|'}, StringSplitOptions.RemoveEmptyEntries );
        foreach (String action in splitActions)
        {
            String[] splitAction = action.Split('=');
            if (splitAction.Length != 2)
            {
                Debug.LogError("action invalid : " + action);    
                continue;
            }
            
            string type = splitAction[0];
            //Debug.Log(action);
            switch (type)
            {
                case "TeleportPlayer":
                    Vector2 teleportPos = ReadPosition(splitAction[1]);
                    TeleportPlayer(teleportPos);
                    break;
                case "Wait":
                    if (float.TryParse(splitAction[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float duration)) 
                        StartCoroutine(Wait(duration));
                    break;
                case "PlayAction":
                    GameManager.character.locomotion.SetTrigger(splitAction[0]);
                    break;
                case "ShowItem":
                    GameManager.character.receiveItem.Receive(GameManager.GetItemSprite(splitAction[1]));
                    break;
                    case "StopShowItem":
                    GameManager.character.receiveItem.Resume();
                    break;
                case "Dialog":
                    ++m_request;
                    GameManager.frame.StartDialog(splitAction[1], EndRequest);
                    break;
                case "StopPlayer":
                    StopPlayer();
                    break;
                case "RestartPlayer":
                    RestartPlayer();
                    break;
                case "ActivateCamera":
                    int cameraId;
                    if (int.TryParse(splitAction[1], out cameraId)) 
                        ActivateCamera(cameraId);
                    break;
                case "CameraTransitionSpeed":
                    if (float.TryParse(splitAction[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float blendDuration))
                        m_camera.m_DefaultBlend.m_Time = blendDuration;
                    break;
                case "EndLevel":
                    GameManager.Exit();
                    break;
            }
        }

    }

    private Vector2 ReadPosition(string _position)
    {
        String[] coordonate = _position.Split(':');

        if (coordonate.Length == 2 && float.TryParse(coordonate[0], NumberStyles.Any, CultureInfo.InvariantCulture, out float x) 
                                   && float.TryParse(coordonate[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float y))
        {
            return new Vector2(x, y);
        }

        Debug.LogError("invalid coordonate : " + _position);
            
        return Vector2.zero;
    }
    
    public void ActivateCamera(int _i)
    {
        foreach (var camera in m_currentCinematic.cameras)
        {
            camera.Priority = 0;
        }
        m_currentCinematic.cameras[_i].Priority = 100;
    }

    private void TeleportPlayer(Vector2 _pos)
    {
        //Debug.Log(m_request + "-> Teleport " + _pos);
        GameManager.character.transform.position = _pos;
    }
    
    private void StopPlayer()
    {
        //Debug.Log(m_request + "-> Stop player");
        GameManager.character.locomotion.enabled = false;
        GameManager.character.velocity = Vector2.zero;
    }
    
    private void RestartPlayer()
    {
        //Debug.Log(m_request + "-> Stop player");
        GameManager.character.locomotion.enabled = true;
    }


    private IEnumerator Wait(float _duration)
    {
        //Debug.Log(m_request + "-> Wait for " + _duration);
        yield return new WaitForSeconds(_duration);
    }

}

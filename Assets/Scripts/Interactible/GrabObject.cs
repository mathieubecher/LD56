using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    private struct LaunchData
    {
        public Transform owner;
        public bool process;
        public Vector2 startingPos;
        public Vector2 startingHeigth;
        public float currentDist;
        public Vector2 direction;
        public float distance;
        public float duration;

        public LaunchData(Transform _owner, Vector2 _startingPos, Vector2 _startingHeigth, Vector2 _direction, float _distance)
        {
            process = true;
            owner = _owner;
            direction = _direction;
            distance = _distance;
            currentDist = 0.0f;
            startingPos = _startingPos;
            startingHeigth = _startingHeigth;
            duration = 0.0f;
        }
    }
    
    [Header("GrabObject")]
    [SerializeField] private Transform m_shadow;
    [SerializeField] private Collider2D m_physic;
    [SerializeField] private Breakable m_breakable;
    [SerializeField] private Transform m_object;
    [Header("Launch")]
    [SerializeField] private AnimationCurve m_releaseTrajectory;
    [SerializeField] private AnimationCurve m_launchTrajectory;
    [SerializeField] private Vector2 m_castBoxSize;
    [SerializeField] private LayerMask m_castLayerMask;
    private Vector2 m_objectOffset;
    private LaunchData m_launchData;

    private void Awake()
    {
        m_objectOffset = m_object.localPosition;
    }

    private void Update()
    {
        if (m_launchData.process)
        {
            m_launchData.duration += Time.deltaTime;

            float maxDuration = m_releaseTrajectory.keys[^1].time;
            float duration = m_launchData.duration;

            float nextDist = m_launchTrajectory.Evaluate(duration / maxDuration);
            transform.position += (Vector3)m_launchData.direction * ((nextDist - m_launchData.currentDist) * m_launchData.distance);
            m_launchData.currentDist = nextDist;
            m_object.localPosition = m_objectOffset + Vector2.Lerp(Vector2.zero, m_launchData.startingHeigth , m_releaseTrajectory.Evaluate(duration));
            if (duration > maxDuration)
            {
                Debug.Log("FIN");
                Hit();
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_launchData.process)
        {
            Vector2 origin = transform.position;
            Vector2 size = m_castBoxSize;
            float angle = 0.0f;
            Vector2 direction = Vector2.zero;
            float distance = 0.0f;
            LayerMask mask = m_castLayerMask;

            bool contact = false;
            RaycastHit2D[] cast = Physics2D.BoxCastAll(origin, size, angle, direction, distance, mask);
            foreach(RaycastHit2D hit in cast)
            {
                if (hit.collider.isTrigger || hit.collider.transform == m_launchData.owner) continue;
                Debug.Log(hit.collider);
                contact = true;
            }

            if (contact)
            {
                Debug.Log("FIN");
                Hit();
            }
        }
    }

    public void Grab()
    {
        if(m_breakable) m_breakable.hurtbox.gameObject.SetActive(false);
        m_shadow.gameObject.SetActive(false);
        m_physic.enabled = false;
    }
    
    public void Release(Transform _owner, Vector2 _pos, Vector2? _direction = null, float _distance = 0.0f)
    {
        m_shadow.gameObject.SetActive(true);
        transform.parent = null;
        transform.localScale = Vector3.one;
        Vector2 grabPos = transform.position;
        transform.position = _pos;
        m_object.position = grabPos + m_objectOffset;
        m_launchData = new LaunchData(_owner, _pos, (Vector2)m_object.localPosition - m_objectOffset, _direction ?? Vector2.zero, _distance);
    }
    
    private void Hit()
    {
        m_launchData.process = false;
        m_object.localPosition = m_objectOffset;
        
        if(m_breakable) m_breakable.Hit(transform.position, 0);
        else Destroy(gameObject);
    }
}

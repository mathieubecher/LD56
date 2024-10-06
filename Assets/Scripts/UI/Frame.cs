using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    [SerializeField] private Character m_character;
    [SerializeField] private List<Sprite> m_lifePoint;
    [SerializeField] private SpriteRenderer m_life;
    [SerializeField] private Vector2 m_offset;
    [SerializeField] private Dialog m_dialog;

    void Start()
    {
        m_dialog.StartDialog(5);
    }
    void Update()
    {
        transform.position = (Vector2)m_character.transform.position + m_offset;
        m_life.sprite = m_lifePoint[m_character.currentLife];
    }
}

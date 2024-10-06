using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowItem : MonoBehaviour
{
    [SerializeField] private string m_item;
    [SerializeField] private SpriteRenderer m_sprite;

    private void Update()
    {
        m_sprite.enabled = (GameManager.HasItem(m_item));
    }
}

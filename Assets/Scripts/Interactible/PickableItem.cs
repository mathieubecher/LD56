using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [SerializeField] private string m_effect;
    
    public void Pick()
    {
        GameManager.PickItem(m_effect);
        Destroy(gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Loot : MonoBehaviour
{
    [Serializable]
    private class Lootable
    {
        public GameObject m_object;
        public float m_lootChance;
    }
    
    [SerializeField] private int m_maxItem = 3;
    [SerializeField] private List<Lootable> m_lootables;
    
    void Start()
    {
        
    }

    public void CreateLoot()
    {
        int nbItems = 0;
        foreach (var lootable in m_lootables)
        {
            if (Random.Range(0.0f, 1.0f) < lootable.m_lootChance)
            {
                ++nbItems;
                Instantiate(lootable.m_object, transform.position, Quaternion.identity);
            }

            if (nbItems > m_maxItem) return;
        }
    }
}

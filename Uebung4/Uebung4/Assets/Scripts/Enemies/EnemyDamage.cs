using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    public bool doesDamageOnTouch;
    public int damage = 20;
    public BoxCollider2D enemyBoxCollider;
    
    private BoxCollider2D m_playerBoxCollider;
    private PlayerHealth m_playerHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        m_playerHealth = GameObject.Find("HeroKnight").GetComponent<PlayerHealth>();
        m_playerBoxCollider = GameObject.Find("HeroKnight").GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyBoxCollider.bounds.Intersects(m_playerBoxCollider.bounds))
        {
            InflictDamage();
        }
    }

    private void InflictDamage()
    {
        if (m_playerHealth != null)
        {
            m_playerHealth.TakeDamage(damage);
        }
    }
}

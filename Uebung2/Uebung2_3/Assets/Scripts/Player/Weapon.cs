using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    private float m_timeSinceAttack = 0.0f;
    private int   m_currentAttack = 0;
    private Animator m_animator;
    
    
    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    
    // Update is called once per frame
    void Update()
    {
        
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        //Sword attack and Sword LASER
        if (Input.GetButtonDown("Fire1") && m_timeSinceAttack > 0.25f)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;

            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
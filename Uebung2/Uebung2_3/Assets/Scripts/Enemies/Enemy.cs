using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;

    public GameObject deathEffect;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        var deathEffectInstance = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(deathEffectInstance,1.5f);
        Destroy(gameObject);
    }
}

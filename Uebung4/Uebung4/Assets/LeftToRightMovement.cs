using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftToRightMovement : MonoBehaviour
{
    public float speed;
    public bool m_FacingRight = true;
    public Animator animator;
    private static readonly int AnimatorSpeed = Animator.StringToHash("Speed");
    public Rigidbody2D enemyRigidBody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        animator.SetFloat(AnimatorSpeed, speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_FacingRight)
        {
            enemyRigidBody2D.AddForce(Vector2.right * (speed * Time.deltaTime));
        }
        else
        {
            enemyRigidBody2D.AddForce(Vector2.left * (speed * Time.deltaTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("TurnPoint"))
        {
            Flip();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Rotates game object on the y axis
        transform.Rotate(0f, 180f, 0f);
    }
}

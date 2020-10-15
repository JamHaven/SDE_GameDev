using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    private float m_horizontalMove = 0f;
    public float runSpeed = 40f;
    public Animator animator;
    private bool m_jump = false;
    private bool m_crouch = false;
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsFalling = Animator.StringToHash("isFalling");

    // Update is called once per frame
    void Update()
    {
        m_horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        
        
        animator.SetFloat(Speed, Mathf.Abs(m_horizontalMove));
        
        if (Input.GetButtonDown("Jump"))
        {
            m_jump = true;
            animator.SetBool(IsJumping, true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            m_crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            m_crouch = false;
        }
    }

    private void LateUpdate()
    {
        animator.SetBool(IsFalling, controller.getRigidbody2D().velocity.y < -0.1 && !controller.GetGrounded());
    }

    public void OnLanding()
    {
        animator.SetBool(IsJumping, false);
        animator.SetBool(IsFalling, false);
    }
    
    private void FixedUpdate()
    {
        controller.Move(m_horizontalMove * Time.fixedDeltaTime, m_crouch, m_jump);
        m_jump = false;
    }
}

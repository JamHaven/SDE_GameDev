using System;
using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;
    //[SerializeField] GameObject m_slideDust;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    //private Sensor_HeroKnight   m_wallSensorR1;
    //private Sensor_HeroKnight   m_wallSensorR2;
    //private Sensor_HeroKnight   m_wallSensorL1;
    //private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private Boolean             m_facingDirectionRight = true;
    private float               m_delayToIdle = 0.0f;


    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        //m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        //m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        //m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        //m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // Update is called once per frame
    void Update ()
    {
        

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            if (!m_facingDirectionRight)
            {
                Flip();
            }
            //GetComponent<SpriteRenderer>().flipX = false;
            
            //m_facingDirectionRight = 1;
        }
            
        else if (inputX < 0)
        {
            if (m_facingDirectionRight)
            {
                Flip();
            }

            //GetComponent<SpriteRenderer>().flipX = true;
            //m_facingDirectionRight = -1;
        }

        // Move
        if (!m_rolling )
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        //m_animator.SetBool("WallSlide", (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State()));

        //Death
        /*if (Input.GetKeyDown("e"))
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }
            
        //Hurt
        else if (Input.GetKeyDown("q"))
            m_animator.SetTrigger("Hurt");
        */
        //Attack
 

        // Block
        /*if (Input.GetMouseButtonDown(1))
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);*/

        // Roll
        /*if (Input.GetKeyDown("left shift") && !m_rolling)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            if (m_facingDirectionRight)
            {
                m_body2d.velocity = new Vector2(1 * m_rollForce, m_body2d.velocity.y);
            }
            else
            {
                m_body2d.velocity = new Vector2(-1 * m_rollForce, m_body2d.velocity.y);
            }
        }*/
            

        //Jump
        if (Input.GetKeyDown("space") && m_grounded)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }
    }

    // Animation Events
    // Called in end of roll animation.
    /*void AE_ResetRoll()
    {
        m_rolling = false;
    }*/

    // Called in slide animation.
    /*void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirectionRight)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            if (m_facingDirectionRight)
            { 
                dust.transform.localScale = new Vector3(1, 1, 1); 
            }
            else
            {
                dust.transform.localScale = new Vector3(-1, 1, 1);
            }
            
        }
    }*/

    private void Flip()
    {
        m_facingDirectionRight = !m_facingDirectionRight;
        
        transform.Rotate(0f, 180f, 0f);
    }
    
}

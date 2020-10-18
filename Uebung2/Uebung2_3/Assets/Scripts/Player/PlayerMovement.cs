using UnityEngine;

namespace Player
{
    /**
     * Controlls the movement of the player and interacts with the controller
     */
    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController2D controller; //Which player controller to use
        public float runSpeed = 40f; // How fast do we move vertically
        public Animator animator; //Which animator conroller to use
        
        private float m_horizontalMove = 0f; //How fast do we move left (negative) or right (positive)
        private bool m_jump = false; //Did we jump?
        private bool m_crouch = false; // Did we crouch TODO: Not finished yet
        
        //Animator parameters
        private static readonly int IsJumping = Animator.StringToHash("isJumping"); //Are we between jumping and landing
        private static readonly int Speed = Animator.StringToHash("Speed"); //How fast are we moving on the x axis
        private static readonly int IsFalling = Animator.StringToHash("isFalling"); //Are we falling?
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded"); // Are we on the ground

        // Update is called once per frame
        private void Update()
        {
            m_horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed; //Define if we move left or right with a certain speed
        
        
            animator.SetFloat(Speed, Mathf.Abs(m_horizontalMove)); //Set Speed parameter in animator
        
            //If we pressed jump, we move vertically later (see FixedUpdate)
            if (Input.GetButtonDown("Jump"))
            {
                m_jump = true;
                animator.SetBool(IsJumping, true); //Tell the animator we are currently jumping
            }
            
            //We are crouched as long as we hold the Crouch button
            if (Input.GetButtonDown("Crouch"))
            {
                m_crouch = true;
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                m_crouch = false;
            }
        
            //Check if we are falling and are on the ground
            animator.SetBool(IsFalling, (controller.GetRigidbody2D().velocity.y < -0.5) && !controller.IsGrounded());
            animator.SetBool(IsGrounded, controller.IsGrounded());


        }

        /**
         * This is called from the controller via event and sets the parameters in the animator controller.
         * Triggered when landing after falling or jumping
         */
        public void OnLanding()
        {
            animator.SetBool(IsJumping, false);
            animator.SetBool(IsFalling, false);
        }
    
        //Handles physics movement and is frame independed
        private void FixedUpdate()
        {
            controller.Move(m_horizontalMove * Time.fixedDeltaTime, m_crouch, m_jump);
            m_jump = false;
        }
    
    }
}

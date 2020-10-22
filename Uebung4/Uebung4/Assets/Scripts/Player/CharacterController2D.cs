using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Player
{
	public class CharacterController2D : MonoBehaviour
	{
		[SerializeField] private float jumpForce = 400f;							// Amount of force added when the player jumps.
		[SerializeField] private float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
		[SerializeField] private float movementSmoothing = .05f;	// How much to smooth out the movement
		[SerializeField] private bool airControl = false;							// Whether or not a player can steer while jumping;
		[SerializeField] private Transform ceilingCheck;							// A position marking where to check for ceilings
		[SerializeField] private Collider2D crouchDisableCollider;				// A collider that will be disabled when crouching
		[SerializeField] private LayerMask platformLayerMask; 					// Which object layers do we detect as Ground
	
		private bool m_Grounded = true;            // Whether or not the player is grounded.
		const float KCeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
		private Rigidbody2D m_Rigidbody2D;
		private bool m_FacingRight = true;  // For determining which way the player is currently facing.
		private Vector3 m_Velocity = Vector3.zero;
		private float m_GroundCheckHeight = 1f; //How far does the Ground check box collider goes down
		private float m_AirTime = 0f; //How long have we been in the air
		private bool m_isDead = false;
		public BoxCollider2D boxCollider;
	
		[FormerlySerializedAs("OnLandEvent")]
		[Header("Events")]
		[Space]

		public UnityEvent onLandEvent;

		[System.Serializable]
		public class BoolEvent : UnityEvent<bool> { }

		public BoolEvent onCrouchEvent;
		private bool m_wasCrouching = false;

		public Rigidbody2D GetRigidbody2D()
		{
			return m_Rigidbody2D;
		}
	
		private void Awake()
		{
			m_Rigidbody2D = GetComponent<Rigidbody2D>();

			if (onLandEvent == null)
				onLandEvent = new UnityEvent();

			if (onCrouchEvent == null)
				onCrouchEvent = new BoolEvent();
		}
	
		private void FixedUpdate()
		{
			CalculateAirTime();
		
			m_Grounded = IsGrounded();
		
			if (IsLanded())
			{
				onLandEvent.Invoke();
			}

		}

		/**
	 * Calculates new air time
	 */
		private void CalculateAirTime()
		{
			if (!IsGrounded())
			{
				m_AirTime += Time.deltaTime;
			}
		}

		/**
	 * Have we just landed on a ground surface?
	 */
		private bool IsLanded()
		{
			if (m_AirTime > 0 && IsGrounded())
			{
				m_AirTime = 0;
				return true;
			}
			return false;
		}
	
		/**
	 * Are we standing on the ground surface, defined by m_PlatformLayerMask?
	 */
		public bool IsGrounded()
		{
			//Creates a box raycast downwards to detect if we are in contact with a ground surface.
			var boxColliderBounds = boxCollider.bounds;
			RaycastHit2D raycastHit = Physics2D.BoxCast(boxColliderBounds.center, boxColliderBounds.size, 0f,
				Vector2.down, m_GroundCheckHeight, platformLayerMask);
			Color rayColor;
			//If we are on the ground we color the rays green, else we color it red
			if (raycastHit.collider != null)
			{
				rayColor = Color.green;
			}
			else
			{
				rayColor = Color.red;
			}
		
			//Draw two lines downward to visualize the BoxCast, the vertical line is not drawn for simplicity
			Debug.DrawRay(boxColliderBounds.center + new Vector3(boxColliderBounds.extents.x,0),Vector2.down * (boxColliderBounds.extents.y +m_GroundCheckHeight), rayColor);
			Debug.DrawRay(boxColliderBounds.center - new Vector3(boxColliderBounds.extents.x,0),Vector2.down * (boxColliderBounds.extents.y +m_GroundCheckHeight), rayColor);

			//returns if the collider detected a ground surface
			return raycastHit.collider != null;
		}

		/**
	 * Moves the game object differently if we are standing, crouching or jumping.
	 */
		public void Move(float move, bool crouch, bool jump)
		{
			if (!m_isDead)
			{
				// If crouching, check to see if the character can stand up
				if (crouch)
				{
					// If the character has a ceiling preventing them from standing up, keep them crouching
					if (Physics2D.OverlapCircle(ceilingCheck.position, KCeilingRadius, platformLayerMask))
					{
						crouch = true;
					}
				}

				//only control the player if grounded or airControl is turned on
				if (m_Grounded || airControl)
				{

					// If crouching
					if (crouch)
					{
						if (!m_wasCrouching)
						{
							m_wasCrouching = true;
							onCrouchEvent.Invoke(true);
						}

						// Reduce the speed by the crouchSpeed multiplier
						move *= crouchSpeed;

						// Disable one of the colliders when crouching
						if (crouchDisableCollider != null)
							crouchDisableCollider.enabled = false;
					}
					else
					{
						// Enable the collider when not crouching
						if (crouchDisableCollider != null)
							crouchDisableCollider.enabled = true;

						if (m_wasCrouching)
						{
							m_wasCrouching = false;
							onCrouchEvent.Invoke(false);
						}
					}

					// Move the character by finding the target velocity
					Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
					// And then smoothing it out and applying it to the character
					m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity,
						movementSmoothing);

					// If the input is moving the player right and the player is facing left...
					if (move > 0 && !m_FacingRight)
					{
						// ... flip the player.
						Flip();
					}
					// Otherwise if the input is moving the player left and the player is facing right...
					else if (move < 0 && m_FacingRight)
					{
						// ... flip the player.
						Flip();
					}
				}

				// If the player should jump...
				if (m_Grounded && jump)
				{
					// Add a vertical force to the player.
					m_Grounded = false;
					m_Rigidbody2D.AddForce(new Vector2(0f, jumpForce));
				}
			}
		}


		/**
	 * Flips the game object and also takes the direction of the child objects with it.
	 * This means, it also rotates, for example, the firePoint to spawn bullets
	 */
		private void Flip()
		{
			// Switch the way the player is labelled as facing.
			m_FacingRight = !m_FacingRight;

			// Rotates game object on the y axis
			transform.Rotate(0f, 180f, 0f);
		}

		public void SetIsDead(bool isDead)
		{
			m_isDead = isDead;
		}

		public bool GetIsDead()
		{
			return m_isDead;
		}
	}
}

using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
	[Header("SET AND DROP")]
	[Space]
	// Amount of force added when the player jumps.
	[SerializeField] private float m_JumpForce = 400f;							
	// How much to smooth out the movement
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	
	// Whether or not a player can steer while jumping;
	[SerializeField] private bool m_AirControl = false;							
	// A mask determining what is ground to the character
	[SerializeField] private LayerMask m_WhatIsGround;							
	// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_GroundCheck;							
	// A position marking where to check for ceilings
	[SerializeField] private Transform m_CeilingCheck;							
	
	

	// Radius of the overlap circle to determine if grounded
	const float k_GroundedRadius = .2f; 
	// Whether or not the player is grounded.
	private bool m_Grounded;            
	// Radius of the overlap circle to determine if the player can stand up
	const float k_CeilingRadius = .2f; 
	// reference to the characters rigidbody 
	private Rigidbody2D m_Rigidbody2D;

	// For determining which way the player is currently facing.
	private bool m_FacingRight = true;  
	// velocity of the chracter 
	private Vector3 m_Velocity = Vector3.zero;

	[Header("EVENTS")]
	[Space]
	public UnityEvent OnLandEvent;
	[System.Serializable] public class BoolEvent : UnityEvent<bool> { }
	public BoolEvent OnClimbEvent;
	//private bool m_wasClimbing = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnClimbEvent == null)
			OnClimbEvent = new BoolEvent();
	}

	// DEDICATED TO PHYSICS
	// Called a *fixed amount of times per second* rather than once per frame (like update)
	private void FixedUpdate(){
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		
		for (int i = 0; i < colliders.Length; i++){
			if (colliders[i].gameObject != gameObject){
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}

	//function responsible for moving all movable game characters
	public void Move(float move, bool jump, bool climb){
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl){
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// User input right and the player is facing left
			if (move > 0 && !m_FacingRight && !climb){
				Flip();
			}
			// user input is left and the player is facing right
			else if (move < 0 && m_FacingRight && !climb){
				Flip();
			}
		}
		// If the player should jump
		// dont allow jumping if climbing
		if (m_Grounded && jump && !climb){
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

	// change the way the player is facing.
	private void Flip(){
		
		m_FacingRight = !m_FacingRight;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
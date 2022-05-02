using UnityEngine;
using UnityEngine.Events;

public class MovementController : MonoBehaviour
{
	[Header("SET AND DROP")]
	[Space]
	// force applied to character jump
	[SerializeField] private float jumpForce = 500f;							
	// movement smoothing
	[Range(0, .3f)] [SerializeField] private float smoothing = .05f;	
	// can user control player position in mid-air?
	[SerializeField] private bool airControl = false;							
	// layers the character treats as ground layers
	[SerializeField] private LayerMask groundLayers;							
	// checks if character is on the ground
	[SerializeField] private Transform groundCheck;							
	// true if character is touching ground layers
	private bool isGrounded;   
	// tells us which way character faces 
	private bool facingRight = true;          
	// reference to the characters rigidbody 
	private Rigidbody2D rb;
	// radius of the circlecast which checks for ground layers 
	const float groundRadius = .2f; 
	// velocity 
	private Vector3 playerVelocity = Vector3.zero;

	[Header("EVENTS")]
	[Space]
	public UnityEvent OnLandEvent;
	[System.Serializable] public class BoolEvent : UnityEvent<bool> { }
	public BoolEvent OnClimbEvent;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnClimbEvent == null)
			OnClimbEvent = new BoolEvent();
	}

	// DEDICATED TO PHYSICS
	// Called a *fixed amount of times per second* rather than once per frame (like update)
	private void FixedUpdate(){
		bool wasGrounded = isGrounded;
		isGrounded = false;

		// circlecast from groundcheck catches anything set as ground
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, groundLayers);
		
		for (int i = 0; i < colliders.Length; i++){
			if (colliders[i].gameObject != gameObject){
				isGrounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}

	//function responsible for moving all movable game characters
	public void Move(float move, bool jump, bool climb){
		if (isGrounded || airControl){
			Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
			// smooth target vaelocity and assign it to character
			rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref playerVelocity, smoothing);

			if (move > 0 && !facingRight && !climb){
				Flip();
			}
			else if (move < 0 && facingRight && !climb){
				Flip();
			}
		}
	
		// dont allow jumping if climbing
		if (isGrounded && jump && !climb){
			isGrounded = false;
			rb.AddForce(new Vector2(0f, jumpForce));
		}
	}

	private void Flip(){
		// flip character to face the opposite direction (x-axis)
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
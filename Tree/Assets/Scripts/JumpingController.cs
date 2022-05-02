using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpingEnemy : MonoBehaviour
{
    [Header("SET AND DROP")]
	[Space]
	// Amount of force added when the player jumps.
	[SerializeField] private float jumpForce = 400f;							
	// A mask determining what is ground to the character
	[SerializeField] private LayerMask whatIsGround;							
	// A position marking where to check if the player is grounded.
	[SerializeField] private Transform groundCheck;	
    									
	
	

	// Radius of the overlap circle to determine if grounded
	const float groundRadius = .2f; 
	// Whether or not the player is grounded.
	private bool isGrounded;            
	// reference to the characters rigidbody 
	[SerializeField] private Rigidbody2D rb;
    // jumping?
    

	[Header("EVENTS")]
	[Space]
	public UnityEvent OnLandEvent;
	

	//private bool m_wasClimbing = false;
    // Start is called before the first frame update
    void Start()
    {
        
        if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
    }

   
    
    private void FixedUpdate(){
		bool wasGrounded = isGrounded;
		isGrounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, whatIsGround);
		
		for (int i = 0; i < colliders.Length; i++){
			if (colliders[i].gameObject != gameObject){
				isGrounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
                    
			}
		}
	}

    public void Jump(bool jump){
         // If the player should jump
		// dont allow jumping if climbing
		if (isGrounded && jump){
			// Add a vertical force to the player.
			isGrounded = false;
			rb.AddForce(new Vector2(0f, jumpForce));
		}
    
    }

    
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleController : MonoBehaviour
{
    [Header("SET AND DROP")]
	[Space]
						
	// How much to smooth out the movement
	[Range(0, .3f)] [SerializeField] private float smoothing = .05f;	
									
	
	// reference to the characters rigidbody 
	private Rigidbody2D rb;

	// For determining which way the player is currently facing.
	private bool facingRight = true;  
	// velocity of the chracter 
	private Vector3 playerVelocity = Vector3.zero;

    public void Move(float move){
        Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
			// And then smoothing it out and applying it to the character
			rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref playerVelocity, smoothing);

			// User input right and the player is facing left
			if (move > 0 && !facingRight){
				Flip();
			}
			// user input is left and the player is facing right
			else if (move < 0 && facingRight){
				Flip();
			}

    }

    // change the way the player is facing.
	private void Flip(){
		
		facingRight = !facingRight;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour {

	[Header("DRAG & DROP")]
	[Space]
	//references the players rigidbody 2D component 
    [SerializeField] private Rigidbody2D rb; 
	// referencing the character controller script.
	public MovementController controller;
	//referencing the Player animator
	public Animator animator; 

	[Header("HORIZONTAL")]
	[Space]
	// speed chatacter runs at
	public float runSpeed = 40f;
	// variables to pass between Update and FixedUpdate
	float horizontalMove = 0f;
	// boolean that tells us if player is jumping
	public bool jump = false;
	

	[Header("CLIMBING")]
	[Space]
	// float for vertical movement
    private float vertical;
    // a float for the speed of climbing
    private float climbSpeed = 3f;
    // a boolean that tells us if we are standing next to the tree trunk
    private bool isTrunk;
    // boolean that indicates whether player is currently climbing
    private bool isClimbing;
	// the y-coordinate where player jumps off tree (used for enemy AI)
	public float exitPoint;
	// boolean telling enemy AI if player has jumped off trunk
	public bool stopClimbing = false;


	[Header("EVENTS")]
	[Space]
	//pop up notification if player uses wrong key to climb
	public UnityEvent ClimbPopUpEvent;
	
	
	
	private void Awake()
	{
		if (ClimbPopUpEvent == null)
			ClimbPopUpEvent = new UnityEvent();

	}
	
	// Update is called once per frame
	// Used to get INPUT from the player instead of MOVING A CHARACTER 
	// specify where we want to move and at what speed
	// every time we get input from a player we use the 'Input' class

	void Update () {
		// GetAxisRaw("Horizontal") is a value between -1 and 1 that changes based on user input
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		//cannot jump while climbing
		if (Input.GetButtonDown("Jump") && !isClimbing)
		{
			jump = true;
			animator.SetBool("IsJumping", true);
		}
		//trying to climb using the wrong key
		if (Input.GetButtonDown("Jump") && isClimbing)
		{
			ClimbPopUpEvent.Invoke();
		}

		//returns a value between -1 and 1 depending on which button is pressed
        vertical = Input.GetAxis("Vertical");
        //if isTrunk and the absolute value of vertical is greater than 0
        if (isTrunk && Mathf.Abs(vertical)!= 0){
            isClimbing = true;
        }

	}

	// DEDICATED TO PHYSICS
	// Called a *fixed amount of times per second* rather than once per frame (like update)
	// APPLY player input to character
	void FixedUpdate (){
		// Takes a float determining how much we want to move: Move(float move, bool jump)
		// set 'move' as the horizontalMove variable received from player input in Update()
		// Time.fixedDeltaTime is the amount of time elapsed since the last time the function was called
		// Multiplying by Time.fixedDeltaTime ensures we always move the same amount no matter how many times the function is called - consistent character speed
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump, isClimbing);
		
		// stops player from jumping forever
		jump = false;

		//set gravity scale of Players Rigidbody2D to 0 when climbing
        if (isClimbing){
            rb.gravityScale = 0f;
            //leave horizontal value as it is but update climbing velocity
            rb.velocity = new Vector2(rb.velocity.x, vertical * climbSpeed);
        }else{
            //return gravity to normal when not climbing
            rb.gravityScale = 3f;
        }	
	}

	//invoked by OnClimbEvent in Character Controller
	public void OnClimbing (bool isClimbing){
		animator.SetBool("IsClimbing", isClimbing);
	}

	//invoked by OnLandEvent in Character Controller
	public void OnLanding (){
		//Debug.Log("landing");
		animator.SetBool("IsJumping", false);
	}

	 //check if player is standing next to the tree trunk
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Trunk")){
			stopClimbing = false;
            isTrunk = true;
            isClimbing = true;
			animator.SetBool("IsJumping", false);
            animator.SetBool("IsClimbing", true);
        }
    }

	//check if player has jumped off the tree trunk
    private void OnTriggerExit2D(Collider2D collision){
        if (collision.CompareTag("Trunk")){
			exitPoint = transform.position.y;
			stopClimbing = true;
            isTrunk = false;
            isClimbing = false;
            animator.SetBool("IsClimbing", false);
        }
    }
}

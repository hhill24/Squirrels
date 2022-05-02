using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    // class to patrol up and down the tree trunk at player y-position
    // used when player moves out of range

    [Header("SET COORDINATES")]
	[Space]
    //position of trunk and ground
    public float LeftTrunk = -10f;
    public float RightTrunk = -13f;
    public float GroundLevel = -2.2f;


    [Header("DRAG & DROP")]
	[Space]
    //references the enemy rigidbody 2D component (drag and drop!!!)
    [SerializeField] private Rigidbody2D rb; 
    // referencing the character controller script.
	public MovementController controller;
    // referencing the enemy animator 
	public Animator animator; 
   
    [Header("HORIZONTAL")]
	[Space]
	// speed chatacter runs at
	public float runSpeed = 50f;
	// variables to pass between Update and FixedUpdate
	float horizontalMove = 0f;
	bool jump = false;

    [Header("CLIMBING")]
	[Space]
	//float for vertical movement
    private float vertical;
    //a float for the speed of climbing
    private float climbSpeed = 6f;
    //a boolean that tells us if we are standing next to the tree trunk
    private bool isTrunk;
    //boolean that indicates whether player is currently climbing
    private bool isClimbing;
	
    [Header("PLAYER INFO")]
	[Space]
	//references the player rigidbody 2D component 
    [SerializeField] private Rigidbody2D playerRb; 

   
	// Update is called once per frame
	// Used to get information from the player character instead of MOVING enemy
	// specify where we want to move and at what speed
	void Update () {

        if (isTrunk && Mathf.Abs(vertical)!=0){
                isClimbing = true;
        }

        //if not on the tree trunk, move towards it
        if (!isClimbing ){
            GoToTrunk();

        }else{
            //move up and down the tree trunk matching thr y-position of the player
            if (rb.position.y < playerRb.position.y && isTrunk){
                vertical = 1;
            }else if (rb.position.y > playerRb.position.y && isTrunk){
                vertical = -1;
            }
        }
        
	}

    //move enemy horizontally towards tree trunk
    public void GoToTrunk(){
        
        if (rb.position.x < RightTrunk){
            horizontalMove = 1 * runSpeed;
        }else if (rb.position.x > LeftTrunk){
            horizontalMove = -1 * runSpeed;
        }
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        
    }
    


	// DEDICATED TO PHYSICS
	// Called a *fixed amount of times per second* rather than once per frame (like update)
	// APPLY player info to update enemy position
	void FixedUpdate(){
		// Time.fixedDeltaTime is the amount of time elapsed since the last time the function was called
		// Multiplying by Time.fixedDeltaTime ensures we always move the same amount no matter how many times the function is called - consistent character speed
		
        rb.velocity = new Vector2(rb.velocity.x, vertical * climbSpeed);

        if (isClimbing){
            rb.gravityScale = 0f;
            //set horizontal parameter to 0 to climb straight up and down
            controller.Move(0, jump, isClimbing); 
        }else{
            //return gravity to normal when not climbing
            rb.gravityScale = 3f;
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump, isClimbing);
        }

		// stops jumping forever!
		jump = false;

	}

    //invoked by OnLandEvent in Character Controller
    public void OnLanding(){
		animator.SetBool("EnemyIsJumping", false);
	}

	//invoked by OnClimbEvent in Character Controller
     public void OnClimbing (bool isClimbing){
		animator.SetBool("EnemyIsClimbing", isClimbing);
	}


	 //check if player is standing next to the tree trunk
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Trunk")){
            isTrunk = true;
            isClimbing = true;
            animator.SetBool("EnemyIsJumping", false);
            animator.SetBool("EnemyIsClimbing", true);
        }	
    }

    private void OnTriggerExit2D(Collider2D collision){
        if (collision.CompareTag("Trunk")){
            isTrunk = false;
            isClimbing = false;
            animator.SetBool("EnemyIsClimbing", false);
        }
    } 
}

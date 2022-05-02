using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // class to chase players exact position
    // used when player moves into enemy range

    [Header("SET COORDINATES")]
	[Space]
    //position of trunk
    public float LeftTrunk = -10f;
    public float RightTrunk = -13f;
    public float GroundLevel = -2.2f;


    [Header("DRAG & DROP")]
	[Space]
    //references the enemy rigidbody 2D component (drag and drop!!!)
    [SerializeField] private Rigidbody2D rb; 
    // referencing the character controller script.
	public MovementController controller;
    //references the enemy animator
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
    private float climbSpeed = 4.5f;
    //a boolean that tells us if we are standing next to the tree trunk
    private bool isTrunk;
    //boolean that indicates whether player is currently climbing
    private bool isClimbing;
	

	[Header("PLAYER INFO")]
	[Space]
    //references the player rigidbody 2D component 
    [SerializeField] private Rigidbody2D playerRb; 
    //references the player movement script
	public PlayerMovement Player;
    //float to store the y-point player jumps off the tree
    public float playerExitPoint;
    // boolean to tell enemy if player has jumped off the tree
    public bool playerStopClimbing;

    

	
	// Update is called once per frame
	// Used to get information from the player character instead of MOVING enemy
	// specify where we want to move and at what speed
	void Update () {

        //check if player has jumped off tree
		playerExitPoint = Player.exitPoint;
        playerStopClimbing = Player.stopClimbing;

        //if player is on the RIGHT of the enemy
        if (rb.position.x < playerRb.position.x ){
        horizontalMove = 1 * runSpeed;
        //if player is on the LEFT of the enemy
        }else if (rb.position.x > playerRb.position.x){
        horizontalMove = -1 * runSpeed;
        }

        //if player jumps, enemy also jumps
        if (Player.jump && !isClimbing)
        {
            jump = true;
            animator.SetBool("EnemyIsJumping", true);
        }


        // player is ABOVE enemy
        if (rb.position.y < playerRb.position.y && isTrunk){
            vertical = 1;
        // player is BELOW enemy
        }else if (rb.position.y > playerRb.position.y && isTrunk){
            vertical = -1;
        }
        
      
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		
        //if isTrunk and the absolute value of vertical is greater than 0
        if (isTrunk && Mathf.Abs(vertical)!=0){
            isClimbing = true;
        }
	}

	// DEDICATED TO PHYSICS
	// Called a *fixed amount of times per second* rather than once per frame (like update)
	// APPLY player info to update enemy position
	void FixedUpdate (){
		// Time.fixedDeltaTime is the amount of time elapsed since the last time the function was called
		// Multiplying by Time.fixedDeltaTime ensures we always move the same amount no matter how many times the function is called - consistent character speed
		
        //enemy is climbing
        //BUGS HERE... CAN ONLY TELL PLAYER EXIT POINT IF ENEMY IS CLIMBING?
        if (isClimbing){
            //set gravity scale of Players Rigidbody2D to 0 when climbing
            rb.gravityScale = 0f;
            
            //if player jumps off the tree trunk
            if(playerStopClimbing){
                if (rb.position.y < playerExitPoint && rb.position.y > GroundLevel){
                    
                    //climb straight up until reaching the point where player jumped off
                    //stops enemy jumping off before the players current branch is beneath them
                    controller.Move(0, jump, isClimbing);
                }else{
                    // exit position reached
                    // set to false as no longer needs to know 
                    Player.stopClimbing = false;
                    //continue to move towards player location as normal
                    rb.velocity = new Vector2(rb.velocity.x, vertical * climbSpeed);
                    controller.Move(horizontalMove * Time.fixedDeltaTime, jump, isClimbing);
                    
                }
            //if player is climbing or running and hasn't recently jumped off the tree     
            }else{
                // move towards player location
                rb.velocity = new Vector2(rb.velocity.x, vertical * climbSpeed);
                controller.Move(horizontalMove * Time.fixedDeltaTime, jump, isClimbing);
            }
           
        // enemy is not climbing
        }else{
            //return gravity to normal when not climbing
            rb.gravityScale = 3f;
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump, isClimbing);
        }

		// stops jumping forever!
        //MAYBE PUT AFTER EACH CALL?
		jump = false;
	}

   
    //invoked by OnLandEvent in Character Controller
    public void OnLanding (){
		animator.SetBool("EnemyIsJumping", false);
	}

	//invoked by OnClimbEvent in Character Controller
     public void OnClimbing (bool isClimbing){
		animator.SetBool("EnemyIsClimbing", isClimbing);
	}

	 //check if enemy is standing next to the tree trunk
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Trunk")){
            isTrunk = true;
            isClimbing = true;
            animator.SetBool("EnemyIsJumping", false);
            animator.SetBool("EnemyIsClimbing", true);
        }	
    }

    //check if enemy has jumped off tree trunk
    private void OnTriggerExit2D(Collider2D collision){
        if (collision.CompareTag("Trunk")){
            isTrunk = false;
            isClimbing = false;
            animator.SetBool("EnemyIsClimbing", false);
			
        }
    }
}

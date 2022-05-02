using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleMovement : MonoBehaviour
{
    
    [Header("DRAG & DROP")]
	[Space]
    //references the player rigidbody 2D component 
    [SerializeField] private Rigidbody2D playerRb; 
    //references the eagle rigidbody 2D component 
    [SerializeField] private Rigidbody2D rb; 
    // speed chatacter flies at
	private float flySpeed = 5f;
    // referencing the character controller script.
	public MovementController controller;
    // the hosizontal movement of the eagle
    public float horizontalMove = 0f;
    //float for vertical movement
    private float vertical;
    
    
    // Update is called once per frame
    void Update()
    {
        if (rb.position.y > -1.84){
        //  MIGHT NEED ENEMY TO HAVE RB FOR THIS??
         //if player is on the RIGHT of the enemy
        if (rb.position.x < playerRb.position.x ){
            flySpeed=20;
            horizontalMove = 1 * flySpeed;
        //if player is on the LEFT of the enemy
        }else if (rb.position.x > playerRb.position.x){
            flySpeed=20;
            horizontalMove = -1 * flySpeed;
        }

        // player is ABOVE enemy
        if (rb.position.y < playerRb.position.y){
            flySpeed=6;
            vertical = 1;
        // player is BELOW enemy
        }else if (rb.position.y > playerRb.position.y){
            flySpeed=6;
            vertical = -1;
        }
        }else{
        vertical = 1;
        }
    }

   

    void FixedUpdate(){
        //rb.gravityScale = 0f;
        rb.velocity = new Vector2(rb.velocity.x, vertical * flySpeed);
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, false);
    }

    
}

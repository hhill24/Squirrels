using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMovement : MonoBehaviour
{
    bool jump = false;
    //references the players rigidbody 2D component 
    [SerializeField] private Rigidbody2D rb; 
    // referencing the character controller script.
	public JumpingController controller;
    //references the enemy animator
	public Animator animator; 

   

    // Start is called before the first frame update
    void Start()
    {
        rb.gravityScale = 3f;
   
    }

    // Update is called once per frame
    void Update()
    {
        jump = true;
        animator.SetBool("EnemyIsJumping", true);
       
    }

    void FixedUpdate(){

        controller.Jump(jump);
		// stops player from jumping forever
		jump = false;
    }

    //invoked by OnLandEvent in Character Controller
    public void OnLanding (){
		animator.SetBool("EnemyIsJumping", false);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleFreeze : MonoBehaviour
{
    [Header("DRAG & DROP")]
	[Space]
    // referEnces to the acorn puzzle script
    [SerializeField] public AcornPuzzle puzzle;
    // referEnces to the enemy rigidbody
    [SerializeField] private Rigidbody2D rb; 
    //references the script that controls all enemy movement 
    public EagleMovement enemyScript;
    // refernces the enemy collider which 'Is Trigger'
    public Collider2D enemycollider;
    // referencing the enemy animator 
	public Animator animator; 
    // boolean true if player is frozen
    public bool isFrozen;
    //sets the time enemy is frozen for
    public float frozenTime;
    



    public void FreezeEnemy(){
        Debug.Log("freezing enemy");
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //enemyScript.FreezeScripts();
            enemyScript.enabled = false;
            enemycollider.enabled = false;
            isFrozen = true;
            animator.enabled=false;
    }

    public void StartCountdown(){
        //wait 5 seconds before unfreezing enemy 
        if(puzzle.won){
            Invoke("UnfreezeEnemy", frozenTime);
            puzzle.won = false;
        }else{
            //unfreeze enemy immediately
            UnfreezeEnemy();
        }
            
    }

    void UnfreezeEnemy(){
        //return enemy to normal
        Debug.Log("unfreezing enemy");
        if(isFrozen){
        
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            enemyScript.enabled = true;
            enemycollider.enabled=true;
            isFrozen = false;
            animator.enabled=true;
        }
        
    }
}

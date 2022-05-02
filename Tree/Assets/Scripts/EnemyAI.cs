using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    
    //----FINITE STATE MACHINE-----//
    
    [Header("DRAG & DROP")]
	[Space]
    // references the player game object
    public GameObject player;
    // referEnces to the enemy movement script
    public EnemyMovement inRangeScript;
    // referEnces to the enemy patrol script
    public EnemyPatrol outOfRangeScript;

    [Header("PLAYER INFO")]
	[Space]
    // stores distance between enemy and player
    public float distanceToPlayer;

  
    // Update is called once per frame
    void Update(){

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < 7 ){
            inRangeScript.enabled = true;
            outOfRangeScript.enabled = false;
        }else{
            inRangeScript.enabled = false;
            outOfRangeScript.enabled = true;
        }
    }

    // stops enemy movement entirely 
    public void FreezeScripts(){
        inRangeScript.enabled = false;
        outOfRangeScript.enabled = false;
    }  
 }

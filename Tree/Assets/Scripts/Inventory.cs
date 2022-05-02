using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("DRAG AND DROP")]
	[Space]
    public Text textAcorns;

    [Header("VARIABLES")]
	[Space]
    public int inventory = 0;

    

    // when player collides with an acorn
    private void OnTriggerEnter2D(Collider2D collision){

        if (collision.CompareTag("Acorn")){
            inventory++;
            textAcorns.text = inventory.ToString();
            Destroy(collision.gameObject);  
        }
    }

    // when player opens an acorn puzzle, decrement inventory
    public void UseAcorn(){
        inventory --;
        textAcorns.text = inventory.ToString();
    }
}

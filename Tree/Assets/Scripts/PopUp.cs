using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    
    [Header("DRAG & DROP")]
	[Space]
    [SerializeField] public GameObject popUp;
    
    [Header("VARIABLES")]
	[Space]
    public int inventory = 0;
    public float time =5f;


    // hide countdown after designated time
    public void startCountdown(){
         Invoke("PopUpHide", time);
    }

    public void PopUpHide(){
        popUp.SetActive(false);
    }
}

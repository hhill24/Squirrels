using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleActive : MonoBehaviour
{
    
    [Header("DRAG & DROP")]
	[Space]
    public Inventory playerInventory;
    public GameObject gameOver;

    [Header("EVENTS")]
	[Space]
    public UnityEvent OnPuzzleEvent;
    public UnityEvent OnNoAcornsEvent;
    

    // Start is called before the first frame update
    private void Awake()
    {
        if (OnPuzzleEvent == null)
			OnPuzzleEvent = new UnityEvent();

        if (OnNoAcornsEvent == null)
			OnNoAcornsEvent = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
		{
			if (playerInventory.inventory == 0){
                OnNoAcornsEvent.Invoke();
            }else if(!gameOver.activeInHierarchy){
                OnPuzzleEvent.Invoke();
            } 
        }
    }
}

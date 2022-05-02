using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour{

    [Header("DRAG & DROP")]
	[Space]
    // displays results of level (won/lost) when gameplay ends
    public Text endText;
    

    [Header("BOOLEANS")]
	[Space]
    bool failed = false;
    public static bool isPaused = false;

    [Header("EVENTS")]
	[Space]
    // invoked when gameplay ends
    public UnityEvent OnCompleteEvent;

    private void Awake(){
		if (OnCompleteEvent == null)
			OnCompleteEvent = new UnityEvent();
	}

    //player collides with enemy or end of level
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Enemy")){
            failed = true;
            OnCompleteEvent.Invoke();
            PauseGame();
            
        }
        else if (collision.CompareTag("Safety")){
            OnCompleteEvent.Invoke();
            PauseGame();
        }
    }

    // update end interface text to match outcome
    public void GameTextUpdate(){
        if (failed){
            endText.text = "Game Over :(";
        }
    }

    //pause time when game ends
    void PauseGame(){
        Time.timeScale = 0f;

    }

    public void ResumeGame(){
        Time.timeScale = 1f;
    }
}


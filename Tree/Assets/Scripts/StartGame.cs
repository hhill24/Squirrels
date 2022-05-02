using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class StartGame : MonoBehaviour
{
    [Header("NEED TO BE SET")]
	[Space]
    //number to count down from
    public int countdown;
    //text for countdown numbers
    public Text countdownText;
    // Pause game UI
    public GameObject pause;
    // Audio clip for background
    [SerializeField] public AudioClip backgroundMusic;
    // Audio clip for success
    [SerializeField] public AudioClip successEffect;
    

    [Header("EVENTS")]
	[Space]
    //sets user interface to not be active
    public UnityEvent OnStartEvent;
    
    // Start is called before the first frame update
    void Start(){
        // enemy.SetActive(false);
        // puzzle.SetActive(false);

        // GameObject Player = Instantiate(Resources.Load("Prefabs/Player", typeof(GameObject)), new Vector3(-17,-3,0), Quaternion.identity) as GameObject;

        // ensure game time is running
        Time.timeScale = 1f;
        
        //  ignore collisions between enemy and enemy layer
        Physics.IgnoreLayerCollision(11, 11, true);
        // ignore collision between player and the eagle enemy ground barrier
        //Physics.IgnoreLayerCollision(6, 12, true);

        StartCoroutine(Countdown());
        if (OnStartEvent == null)
			OnStartEvent = new UnityEvent();
    }

    void Update(){
        if (Input.GetButtonDown("Pause")){
            pause.SetActive(true);
            PauseGame();

        }
    }


    //countdown at start of game
    IEnumerator Countdown(){
        while (countdown >0){
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }
        OnStartEvent.Invoke();
    }

    public void PauseGame(){
        Time.timeScale = 0f;
    }
}

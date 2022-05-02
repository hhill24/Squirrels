using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //the player
    public Transform target;

    public Vector3 offset;
    public Vector3 startPosition;
    public GameObject puzzle;
    public GameObject game;
    

    //the higher the value the faster the camera locks onto target
    public float smoothSpeed = 0.125f;

    void Start(){
        startPosition = transform.position;
    }


    void FixedUpdate(){
        if(game.activeInHierarchy){
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
        if(puzzle.activeInHierarchy){
            transform.position = startPosition;
        }
        

    }
}

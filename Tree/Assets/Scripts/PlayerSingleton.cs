using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
   static PlayerSingleton instance;

   void Awake(){

       if (instance != null){
           Destroy(gameObject);
       }else{
           instance = this;
           DontDestroyOnLoad(gameObject);
       }
       
    }   
}

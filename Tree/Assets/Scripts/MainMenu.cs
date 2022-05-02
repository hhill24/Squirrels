using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void HomeMenu(){
        SceneManager.LoadScene(0);
    }

    public void RetryLevel(){
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void EasyLevel(){
        SceneManager.LoadScene(1);
    }

    public void MediumLevel(){
        SceneManager.LoadScene(2);
    }

    public void DifficultLevel(){
        SceneManager.LoadScene(3);
    }

    public void QuitGame(){
        Debug.Log("QUIT");
        Application.Quit();  
    }
} 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseGame : MonoBehaviour
{
    
    public void CloseGameButton(){
        #if UNITY_WEBGL && !UNITY_EDITOR
            SceneManager.LoadScene("RAInput");
        #else
            Application.Quit();
        #endif
    }
}

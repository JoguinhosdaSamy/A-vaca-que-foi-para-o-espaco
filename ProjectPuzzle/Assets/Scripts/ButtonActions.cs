using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    
    public ButtonActions ButtonPlay;



    public void Play()
    {
         SceneManager.LoadScene ("Fase Teste");
    }

    private void OnButtonPlayClick()
    {
       Play();
    }
}

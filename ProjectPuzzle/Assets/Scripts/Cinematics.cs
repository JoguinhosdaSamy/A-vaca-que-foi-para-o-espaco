using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematics : MonoBehaviour
{
    private GameObject _audioManager;
    private GameObject _saveManager;
    private GameObject _pauseMenu;
    
    private void Start()
    {
        _audioManager = GameObject.Find("AudioManager");
        _saveManager = GameObject.Find("SaveManager");
        _pauseMenu = GameObject.Find("Canvas Pause Menu");
        
        if(_audioManager != null)
            Destroy(_audioManager);
        
        if(_saveManager != null)
            Destroy(_saveManager);
        
        if(_pauseMenu != null)
            Destroy(_pauseMenu);
    }
}

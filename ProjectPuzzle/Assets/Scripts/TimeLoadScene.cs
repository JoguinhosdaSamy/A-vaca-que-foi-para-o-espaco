using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeLoadScene : MonoBehaviour
{

    public string scene;
    public float time;



    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadScene", time); 
    }
    void LoadScene()
    {
        SceneManager.LoadScene(scene);
    }
}

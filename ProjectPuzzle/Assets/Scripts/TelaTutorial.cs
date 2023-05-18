using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelaTutorial : MonoBehaviour
{
    public GameObject[] telas;

    private int NumTelas = 0, Atual = 0;

    private void Start()
    {
        NumTelas = transform.childCount;
        Atual = 1;
        transform.GetChild(Atual).gameObject.SetActive(true);
    }

    private void Update()
    {

        if (Input.touchCount > 0 && Input.GetMouseButtonDown(0))
        {
            Atual++;
            if(Atual < NumTelas)
            {
                
                transform.GetChild(Atual-1).gameObject.SetActive(false);
                transform.GetChild(Atual).gameObject.SetActive(true);
            }
            else
            {
                transform.gameObject.SetActive(false);
                GameController.controller.Tutorial = false;
            }

        }

    }

}

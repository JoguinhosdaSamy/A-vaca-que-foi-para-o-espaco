using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuFases : MonoBehaviour
{
    public Button[] faseButtons; 

    private void Update()
    {
        for (int i = 0; i < faseButtons.Length; i++)
        {
          if(i+3 > PlayerPrefs.GetInt("faseCompletada"))
          {
            faseButtons[i].interactable = false;
          }
        }
    }

}

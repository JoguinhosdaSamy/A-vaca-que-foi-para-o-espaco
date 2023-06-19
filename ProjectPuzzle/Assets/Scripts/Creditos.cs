using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
    public GameObject creditsPanel; 
    public string menuSceneName; 

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void GoBackToMenu()
    {
        creditsPanel.SetActive(false);

        SceneManager.LoadScene(menuSceneName);

    }

}

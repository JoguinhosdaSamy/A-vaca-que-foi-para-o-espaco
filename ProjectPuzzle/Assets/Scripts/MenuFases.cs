using Save;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuFases : MonoBehaviour
{
    public Button[] faseButtons; 

    private void Start()
    {
        DisableButtonsForNonCompletedFases();
    }

    private void DisableButtonsForNonCompletedFases()
    {
        foreach (Button button in faseButtons)
        {
            string nomeFase = button.gameObject.name; 

            FaseInfo faseInfo = SaveManager.ReadFaseData(nomeFase); 

            if (faseInfo == null)
            {
                button.interactable = false;
            }
        }
    }

}

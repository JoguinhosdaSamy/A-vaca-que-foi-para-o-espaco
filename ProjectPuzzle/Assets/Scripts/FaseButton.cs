using UnityEngine;
using UnityEngine.SceneManagement;

public class FaseButton : MonoBehaviour
{
  public void CarregarCena(string nomeDaCena)
  {
    Debug.Log("Carregando cena: " + nomeDaCena);
    SceneManager.LoadScene(nomeDaCena);
  }

}

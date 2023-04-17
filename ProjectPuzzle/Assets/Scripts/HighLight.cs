using System.Collections;
using UnityEngine;

public class Highlight : MonoBehaviour
{
  public float blinkSpeed = 0.1f; // A velocidade do piscar
  public float blinkDuration = 1.0f; // A duração do piscar
  public Material defaultMaterial; // O material padrão do objeto
  public Material blinkingMaterial; // O material que faz o objeto piscar
  private Renderer rend;
  private bool isBlinking = false; // Flag que indica se o objeto está piscando ou não

  void Start() 
  {
    rend = GetComponent<Renderer>();
    rend.material = defaultMaterial;
  }

  void Update() 
  {
    RaycastHit hit;
    if (Physics.Raycast(transform.position, transform.forward, out hit))
    {
        if (hit.transform.CompareTag("Player"))
        {
            if (!isBlinking)
            {
                StartCoroutine(Blink());
            }
        }
    }
  }

  IEnumerator Blink() 
  {
    isBlinking = true;
    float time = 0;
    while (time < blinkDuration)
    {
        rend.material = blinkingMaterial;
        yield return new WaitForSeconds(blinkSpeed);
        rend.material = defaultMaterial;
        yield return new WaitForSeconds(blinkSpeed);
        time += 2 * blinkSpeed;
    }
    isBlinking = false;
  }
}  





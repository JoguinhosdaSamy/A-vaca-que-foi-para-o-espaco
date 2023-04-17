using UnityEngine;

public class Highlight : MonoBehaviour
{
    private static Highlight instance;

    // Material usado para destacar o objeto
    public Material highlightMaterial;

    // Salva o material original do objeto
    private Material originalMaterial;

    // Armazena o objeto atualmente em destaque
    private GameObject currentHighlightedObject;

    // Obtém a instância única do script
    public static Highlight GetInstance() {
        if (instance == null) {
            instance = new Highlight();
        }
        return instance;
    }

    // Destaca o objeto especificado
    public void HighlightObject(GameObject objectToHighlight) {
        // Remove o highlight do objeto anterior
        RemoveHighlight();

        // Armazena o objeto atualmente em destaque
        currentHighlightedObject = objectToHighlight;

        // Salva o material original do objeto
        originalMaterial = objectToHighlight.GetComponent<Renderer>().material;

        // Define o material de highlight no objeto
        objectToHighlight.GetComponent<Renderer>().material = highlightMaterial;
    }

    // Remove o highlight do objeto atualmente em destaque
    public void RemoveHighlight() {
        if (currentHighlightedObject != null) {
            // Restaura o material original do objeto
            currentHighlightedObject.GetComponent<Renderer>().material = originalMaterial;

            // Limpa a referência do objeto atualmente em destaque
            currentHighlightedObject = null;
        }
    }
}

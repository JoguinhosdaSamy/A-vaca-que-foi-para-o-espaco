using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLight : MonoBehaviour
{
    public Material playerHighlightMaterial;
    public Material IAHighlightMaterial;

    private Material originalMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            originalMaterial = GetComponent<MeshRenderer>().material;
            GetComponent<MeshRenderer>().material = playerHighlightMaterial;
        }
        else if (other.CompareTag("Enemy"))
        {
            originalMaterial = GetComponent<MeshRenderer>().material;
            GetComponent<MeshRenderer>().material = IAHighlightMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponent<MeshRenderer>().material = originalMaterial;
    }
}


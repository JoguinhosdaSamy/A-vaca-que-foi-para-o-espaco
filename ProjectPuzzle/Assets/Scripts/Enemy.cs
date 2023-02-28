using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector3 playerPosition;
    public Vector3 targetPosition;

    void Update()
    {
        float speed = 5.0f; // velocidade da IA
        Vector3 direction = (playerPosition - targetPosition).normalized;
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;
        transform.position = newPosition;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum Movement {Moving, Alien, Vaca};
    public Movement MovementStatus ;
    [HideInInspector] public Enemy Enemy;
    
    
    void Start()
    {
        Enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        MovementStatus = Movement.Moving;
    }

    public void SetMovementStatus(Movement tipo)
    {
        if (tipo == Movement.Alien)
        {
            Enemy.FindShortestPath();
        }
        MovementStatus = tipo;
    }
    public void Victory()
    {
        Debug.Log("");
    }
}
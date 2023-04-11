using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private GameController controller;
    public Point target;
    public static Player player;
    public Enemy enemy;
    [HideInInspector]
    public bool _istargetNull;

    void Start()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        _istargetNull = target == null;
        player = this;
    }

    void FixedUpdate()
     {
         if (target == enemy.currentPoint)
         {
             if(controller.MovementStatus == GameController.Movement.Vaca){
                 controller.GameOver();
             }
        
         }
         
         if (_istargetNull) return;

         Vector3 dir = target.transform.position - transform.position;
         if (dir.magnitude > 0.1)
         {
            transform.Translate(dir.normalized * (3.0f * Time.deltaTime), Space.World);
            Vector3 rot = dir.normalized;
            rot.y = transform.position.y;
            transform.rotation = Quaternion.LookRotation(dir.normalized);
         } else {
             _istargetNull = true;
             controller.SetMovementStatus(GameController.Movement.Alien);
             CheckCondition();
         }
     }
    private void CheckCondition()
    {
       
        var targetTag = target.Property;

        switch (targetTag)
        {
            case Point.Prop.EndPoint:
                controller.Victory();
                break;
            case Point.Prop.SleepPower:
                controller.PowerSleep();
                break;
        }
        
    }
}
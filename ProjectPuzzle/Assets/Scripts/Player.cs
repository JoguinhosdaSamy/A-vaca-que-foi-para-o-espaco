using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
                     CheckDie();
                     CheckVictory();
                 }
         }
    void CheckVictory()
    {
        GameObject finishObject = GameObject.FindGameObjectWithTag("Finish");

        if (finishObject != null)
        {
            float distanceToFinish = Vector3.Distance(transform.position, finishObject.transform.position);
            if (distanceToFinish < 0.1f)
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }


    void CheckDie()
    {
        if (target == enemy.pointActual)
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

}
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
     
                 if (_istargetNull) return;
     
                 Vector3 dir = target.transform.position - transform.position;
                 if (dir.magnitude > 0.1)
                 {
                     transform.Translate(dir.normalized * (3.0f * Time.deltaTime), Space.World);
                 } else {
                     _istargetNull = true;
                     controller.SetMovementStatus(GameController.Movement.Alien);
                     CheckDie();
                     CheckVictory();
                 }
         }
    void CheckVictory()
    {
        if (target.endPoint)
        {
            Debug.Log("Aqui a gente troca de scena de acordo com a scena que tiver no gameController");
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